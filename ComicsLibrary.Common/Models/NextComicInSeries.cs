﻿namespace ComicsLibrary.Common
{

    public class NextComicInSeries
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string SeriesTitle { get; set; }
        public string IssueTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ReadUrl { get; set; }
        public int UnreadBooks { get; set; }
        public string Creators { get; set; }
        public int Progress { get; set; }
    }
}
