﻿namespace ComicsLibrary.Common.Data
{
    public class HomeBookType
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public int BookTypeId { get; set; }
        public bool Enabled { get; set; }

        public Series Series { get; set; }
        public BookType BookType { get; set; }
    }
}
