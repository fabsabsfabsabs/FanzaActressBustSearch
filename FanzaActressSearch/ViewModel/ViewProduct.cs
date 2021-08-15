namespace FanzaActressSearch.ViewModel
{
    public record ViewProduct
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlSmall { get; set; }
        public string ImageUrlLarge { get; set; }
        public string SampleImageURL { get; set; }
        public int SampleImageURLCount { get; set; }
        public string SampleMovieLURL { get; set; }
        public string SampleMovieMURL { get; set; }
        public string SampleMovieSURL { get; set; }
        public string Date { get; set; }
        public int Volume { get; set; }
        public string GenreName { get; set; }
        public string ActressName { get; set; }
        public string HasMoviePrevId { get; set; }
        public string HasMovieNextId { get; set; }
        public string HasMovieNumber { get; set; }
    }
}
