﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;

using ApiComic = ComicsLibrary.Common.Api.Comic;
using ApiSeries = ComicsLibrary.Common.Api.Series;
using Comic = ComicsLibrary.Common.Models.Comic;
using Series = ComicsLibrary.Common.Models.Series;
using ComicsLibrary.Common.Models;

namespace ComicsLibrary.Services
{
    public class Service : IService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly IMapper _mapper;
        private readonly IApiService _apiService;
        private readonly ILogger _logger;

        public Service(Func<IUnitOfWork> unitOfWorkFactory, IMapper mapper,
            IApiService apiService, ILogger logger)
        {
            _apiService = apiService;
            _logger = logger;
            _mapper = mapper;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void AbandonSeries(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Series>().GetById(id);
                series.Abandoned = true;
                uow.Save();
            }
        }

        public async Task<int> AddSeriesToLibrary(ApiSeries series)
        {
            try
            {
                using (var uow = _unitOfWorkFactory())
                {
                    var s = _mapper.Map<ApiSeries, Series>(series);
                    s.LastUpdated = DateTime.Now;
                    s.Comics = await _apiService.GetAllSeriesComicsAsync(series.MarvelId);
                    foreach (var c in s.Comics)
                    {
                        c.DateAdded = DateTime.Now;
                    }
                    uow.Repository<Series>().Insert(s);
                    uow.Save();
                    return s.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return 0;
            }
        }

        public ApiSeries GetSeries(int seriesId, int numberOfComics)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = _mapper.Map<Series, ApiSeries>(uow.Repository<Series>()
                    .Including(s => s.Comics)
                    .Single(c => c.Id == seriesId));

                series.Issues = series.Issues
                    .OrderByDescending(c => c.IssueNumber)
                    .Take(numberOfComics)
                    .ToArray();

                return series;
            }
        }

        public List<ApiComic> GetComics(int seriesId, int limit, int offset)
        {
            using (var uow = _unitOfWorkFactory())
            {
                return _mapper.Map<List<Comic>, List<ApiComic>>(uow.Repository<Comic>()
                    .Including(s => s.Series)
                    .Where(s => s.SeriesId == seriesId)
                    .OrderByDescending(c => c.IssueNumber)
                    .Skip(offset)
                    .Take(limit)
                    .ToList());
            }
        }

        public async Task<PagedResult<ApiComic>> GetComicsByMarvelId(int marvelId, int limit, int offset)
        {
            try
            {
                var page = offset / limit + 1;
                var result = await _apiService.GetSeriesComicsAsync(marvelId, limit, page);
                var comics = _mapper.Map<List<Comic>, List<ApiComic>>(result.Results);
                return new PagedResult<ApiComic>(comics, limit, page, result.Total);
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return null;
            }
        }

        public List<ApiSeries> GetSeriesByStatus(SeriesStatus status)
        {
            using (var uow = _unitOfWorkFactory())
            {
                // TODO - change to prevent fetching all comics from each series - only need the first

                var series = status == SeriesStatus.Reading
                    ? GetSeriesInProgress(uow)
                    : status == SeriesStatus.ToRead
                    ? GetSeriesToRead(uow)
                    : status == SeriesStatus.Read
                    ? GetSeriesFinished(uow)
                    : GetSeriesArchived(uow);

                var list = series.OrderBy(s => s.Title)
                    .ToList()
                    .Select(c => _mapper.Map<Series, ApiSeries>(c))
                    .ToList();

                if (status == SeriesStatus.ToRead)
                {
                    list.ForEach(s => s.Progress = 0);
                }
                else if (status == SeriesStatus.Read)
                {
                    list.ForEach(s => s.Progress = 100);
                }

                return list;
            }
        }

        public NextComicInSeries MarkAsRead(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comic = uow.Repository<Comic>().GetById(id);
                comic.IsRead = true;
                uow.Save();

                var next = uow.Repository<Comic>()
                    .Including(c => c.Series)
                    .Where(c => c.SeriesId == comic.SeriesId && !c.IsRead)
                    .OrderBy(c => c.OnSaleDate)
                    .FirstOrDefault();

                if (next == null)
                    return null;

                return _mapper.Map<Comic, NextComicInSeries>(next);
            }
        }

        public void MarkAsUnread(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comic = uow.Repository<Comic>().GetById(id);
                comic.IsRead = false;
                uow.Save();
            }
        }

        public void ReinstateSeries(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Series>().GetById(id);
                series.Abandoned = false;
                uow.Save();
            }
        }


        public void RemoveSeriesFromLibrary(int id)
        {
            using (var uow = _unitOfWorkFactory())
            {
                var comicsToDelete = uow.Repository<Comic>().Where(c => c.SeriesId == id).Select(c => c.Id).ToList();

                foreach (var comic in comicsToDelete)
                {
                    uow.Repository<Comic>().Delete(comic);
                }

                uow.Repository<Series>().Delete(id);
                uow.Save();
            }
        }

        public async Task UpdateSeries(int numberToUpdate)
        {
            try
            {
                using (var uow = _unitOfWorkFactory())
                {
                    var weekAgo = DateTime.Now.AddDays(-7);
                    var yearAgo = DateTime.Now.AddYears(-1);

                    var ongoingSeries = uow.Repository<Series>()
                        .Where(s => s.MarvelId.HasValue
                            && s.LastUpdated < weekAgo
                            && (s.Comics.Any(c => c.OnSaleDate > yearAgo)
                                || s.Comics.Any(c => string.IsNullOrEmpty(c.ReadUrl))));

                    var totalOngoingSeries = ongoingSeries.Count();

                    if (totalOngoingSeries == 0)
                        return;

                    var seriesToUpdate = ongoingSeries
                        .OrderBy(s => s.LastUpdated)
                        .Take(numberToUpdate)
                        .ToList();

                    foreach (var series in seriesToUpdate)
                    {
                        await UpdateSeries(uow, series);
                    }

                    uow.Save();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public async Task<PagedResult<ApiSeries>> SearchByTitle(string title, int sortOrder, int limit, int page)
        {
            var searchResults = await _apiService.SearchSeriesAsync(title, limit, page, (SearchOrder)sortOrder);

            using (var uow = _unitOfWorkFactory())
            {
                var inLibrary = uow.Repository<Series>()
                    .Where(s => s.MarvelId.HasValue)
                    .ToDictionary(s => s.MarvelId.Value, s => s.Id);

                var series = new List<ApiSeries>();
                foreach (var result in searchResults.Results)
                {
                    if (!result.MarvelId.HasValue)
                        continue;

                    inLibrary.TryGetValue(result.MarvelId.Value, out int libraryId);

                    series.Add(new ApiSeries
                    {
                        MarvelId = result.MarvelId.Value,
                        Title = result.Title,
                        StartYear = result.StartYear,
                        EndYear = result.EndYear,
                        Type = result.Type,
                        ImageUrl = result.ImageUrl,
                        Id = libraryId,
                        Url = result.Url
                    });
                }
                return new PagedResult<ApiSeries>(series, limit, page, searchResults.Total);
            }
        }

        private async Task UpdateSeries(IUnitOfWork uow, Series series)
        {
            try
            {
                var newSeriesUpdateTime = DateTime.Now;
                var updatedComics = await _apiService.GetAllSeriesComicsAsync(series.MarvelId.Value);
                foreach (var comic in updatedComics)
                {
                    UpdateComic(uow, comic, series.Id);
                }
                series.LastUpdated = newSeriesUpdateTime;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", series.Id);
                _logger.Log(ex);
            }
        }

        private void UpdateComic(IUnitOfWork uow, Comic comic, int seriesId)
        {
            try
            {
                if (string.IsNullOrEmpty(comic.ImageUrl) || comic.Title.EndsWith("Variant)"))
                    return;

                var savedComic = uow.Repository<Comic>().SingleOrDefault(c => c.MarvelId == comic.MarvelId);

                if (savedComic == null)
                {
                    comic.SeriesId = seriesId;
                    comic.DateAdded = DateTime.Now;
                    uow.Repository<Comic>().Insert(comic);
                }
                else
                {
                    _mapper.Map(comic, savedComic);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Series ID", seriesId);
                ex.Data.Add("Comic Issue Number", comic.IssueNumber.HasValue ? comic.IssueNumber.ToString() : "null");
                ex.Data.Add("Comic Marvel ID", comic.MarvelId.HasValue ? comic.MarvelId.ToString() : "null");
                _logger.Log(ex);
            }
        }

        public List<NextComicInSeries> GetAllNextIssues()
        {
            using (var uow = _unitOfWorkFactory())
            {
                var series = uow.Repository<Comic>()
                    .Including(c => c.Series)
                    .Where(c => !c.Series.Abandoned && !c.IsRead)
                    .OrderBy(c => c.OnSaleDate)
                    .ToList();

                var groups = series
                     .GroupBy(c => c.SeriesId)
                     .Select(g => new { Unread = g.Count(), Next = g.First() })
                     .OrderBy(c => c.Next.Series.Title)
                     .ToList();

                var list = new List<NextComicInSeries>();

                foreach (var g in groups)
                {
                    var comic = _mapper.Map<Comic, NextComicInSeries>(g.Next);
                    comic.UnreadIssues = g.Unread;
                    list.Add(comic);
                }

                return list;
            }
        }

        private IEnumerable<Series> GetSeriesInProgress(IUnitOfWork uow)
        {
            return uow.Repository<Series>().Including(s => s.Comics).Where(s => !s.Abandoned && s.Comics.Any(c => c.IsRead) && s.Comics.Any(c => !c.IsRead));
        }

        private IEnumerable<Series> GetSeriesToRead(IUnitOfWork uow)
        {
            return uow.Repository<Series>().Including(s => s.Comics).Where(s => !s.Abandoned && s.Comics.All(c => !c.IsRead));
        }

        private IEnumerable<Series> GetSeriesFinished(IUnitOfWork uow)
        {
            return uow.Repository<Series>().Including(s => s.Comics).Where(s => !s.Abandoned && s.Comics.All(c => c.IsRead));
        }

        private IEnumerable<Series> GetSeriesArchived(IUnitOfWork uow)
        {
            return uow.Repository<Series>().Including(s => s.Comics).Where(s => s.Abandoned);
        }
    }
}
