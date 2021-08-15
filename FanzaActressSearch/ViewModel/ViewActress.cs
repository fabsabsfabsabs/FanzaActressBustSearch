namespace FanzaActressSearch.ViewModel
{
    public record ViewActress
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ruby { get; set; }
        public string Bust { get; set; }
        public string Hip { get; set; }
        public string Waist { get; set; }
        public string Cup { get; set; }
        public string ImageSURL { get; set; }
        public string ImageMURL { get; set; }
        public string DigitalURL { get; set; }
        public bool IsHeart { get; set; }
        public bool IsNew { get; set; }
        public int WorkCounter { get; set; }
        public string FirstSinglePeriod { get; set; }
        public string LastSinglePeriod { get; set; }
        public string TwitterName { get; set; }
        public string InstagramName { get; set; }
    }
}
