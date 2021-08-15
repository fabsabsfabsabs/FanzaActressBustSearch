namespace FanzaActressSearch.ViewModel
{
    public record ViewProductOne
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public string ImageUrlLarge { get; set; }
        public string SampleImageURL { get; set; }
        public int SampleImageURLCount { get; set; }
        public string SampleMoviePcURL { get; set; }
        public string SampleMovieSpURL { get; set; }
        public string Date { get; set; }
        public int Volume { get; set; }
        public string GenreNames { get; set; }
        public string ActressNames { get; set; }
        public string SeriesNames { get; set; }
        public string MakerNames { get; set; }
    }
}
