namespace DapperDashboardProject.Models
{
    public class GoogleNewsApiResponse
    {
        public List<GoogleNewsItem> Items { get; set; }
    }

    public class GoogleNewsItem
    {
        public string Timestamp { get; set; }
        public string Title { get; set; }
        public string Snippet { get; set; }
        public GoogleNewsImages Images { get; set; }
        public string NewsUrl { get; set; }
        public string Publisher { get; set; }

        public string NewsDate =>
            DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(Timestamp))
                          .ToString("dd.MM.yyyy HH:mm");
    }

    public class GoogleNewsImages
    {
        public string Thumbnail { get; set; }
        public string ThumbnailProxied { get; set; }
    }

}
