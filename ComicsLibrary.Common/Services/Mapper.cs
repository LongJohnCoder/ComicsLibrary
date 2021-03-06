﻿using ComicsLibrary.Common.Data;
using System.Linq;
using Series = ComicsLibrary.Common.Data.Series;

namespace ComicsLibrary.Common
{
    public class Mapper : IMapper
    {
        public LibrarySeries Map(Series source)
        {
            return new LibrarySeries
            {
                Id = source.Id,
                Title = source.Title,
                ImageUrl = source.GetImageUrl(),
                Progress = source.GetProgress(),
                Archived = source.Abandoned,
            };
        }

        public Comic Map(Book source)
        {
            return new Comic
            {
                Id = source.Id,
                Title = source.Title,
                SeriesId = source.SeriesId,
                SeriesTitle = source.Series?.Title,
                IssueTitle = source.GetBookTitle(),
                SourceID = source.Series?.SourceId ?? 0,
                SourceItemID = source.SourceItemID,
                SourceName = source.Series?.Source?.Name,
                TypeID = source.BookType?.ID ?? 0,
                TypeName = source.BookType?.Name,
                IsRead = source.DateRead.HasValue,
                OnSaleDate = source.OnSaleDate.HasValue
                    ? source.OnSaleDate.Value.Date.ToShortDateString()
                    : "",
                DateAdded = source.DateAdded,
                ReadUrlAdded = source.ReadUrlAdded,
                ImageUrl = source.ImageUrl,
                ReadUrl = source.ReadUrl,
                IssueNumber = source.Number,
                DateRead = source.DateRead,
                Creators = source.Creators,
                Hidden = source.Hidden
            };
        }

        public Comic Map(SeriesBook source)
        {
            return new Comic
            {
                Id = source.Id,
                Title = source.Title,
                IssueTitle = source.GetBookTitle(),
                TypeID = source.BookTypeID,
                IsRead = source.IsRead,
                ImageUrl = source.ImageUrl,
                ReadUrl = source.ReadUrl,
                IssueNumber = source.Number,
                Hidden = source.Hidden
            };
        }
    }
}
