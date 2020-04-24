﻿using System.Collections.Generic;
using ComicsLibrary.Common.Api;

namespace ComicsLibrary.Common.Interfaces
{
    public interface ISeriesService
    {
        List<Comic> GetBooks(int seriesId, int typeId, int limit, int offset);
        List<Api.Series> GetByStatus(SeriesStatus status);
        Api.Series GetById(int id, int numberOfComics);
        void Reinstate(int id);
        void Archive(int id);
        void Remove(int id);

    }
}
