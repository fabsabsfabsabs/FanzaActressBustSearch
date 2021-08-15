namespace FanzaActressSearch.ViewModel
{
    public record ViewActressOne
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ruby { get; set; }
        public string Bust { get; set; }
        public string Waist { get; set; }
        public string Hip { get; set; }
        public string Cup { get; set; }
        public string Birthday { get; set; }
        public string BloodType { get; set; }
        public string Hobby { get; set; }
        public string Prefectures { get; set; }
        public string ImageURL { get; set; }
        public string DigitalURL { get; set; }
        public string MonthlyURL { get; set; }
        public string MonoURL { get; set; }
        public string RentalURL { get; set; }
        public int WorkCounter { get; set; }
        public string Product { get; set; }
        public string FirstSinglePeriod { get; set; }
        public string LastSinglePeriod { get; set; }
        public string Wiki { get; set; }
        public string TwitterName { get; set; }
        public string InstagramName { get; set; }
        public PaginatedList<ViewProduct> ViewProductList { get; set; }
    }
}
