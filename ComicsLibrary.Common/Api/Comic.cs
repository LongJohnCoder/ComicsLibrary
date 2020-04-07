﻿using System;

namespace ComicsLibrary.Common.Api
{
    public class Comic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SeriesTitle { get; set; }
        public string IssueTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ReadUrl { get; set; }
        public double? IssueNumber { get; set; }
        public int SeriesId { get; set; }
        public string OnSaleDate { get; set; }
        public bool IsRead { get; set; }
        public bool ToReadNext { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? ReadUrlAdded { get; set; }
        public DateTime? DateRead { get; set; }
        public string Creators { get; set; }
    }
}
