namespace ActressGetter.Dmm
{
    public class ProductSearchJson
    {
        public ProductRequest request { get; set; }
        public ProductResult result { get; set; }
    }

    public class ProductRequest
    {
        public ProductParameters parameters { get; set; }
    }

    public class ProductParameters
    {
        public string api_id { get; set; }
        public string affiliate_id { get; set; }
        public string site { get; set; }
        public string service { get; set; }
        public string floor { get; set; }
        public string keyword { get; set; }
    }

    public class ProductResult
    {
        public int status { get; set; }
        public int result_count { get; set; }
        public int total_count { get; set; }
        public int first_position { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string service_code { get; set; }
        public string service_name { get; set; }
        public string floor_code { get; set; }
        public string floor_name { get; set; }
        public string category_name { get; set; }
        public string content_id { get; set; }
        public string product_id { get; set; }
        public string title { get; set; }
        public string volume { get; set; }
        public Review review { get; set; }
        public string URL { get; set; }
        public string URLsp { get; set; }
        public string affiliateURL { get; set; }
        public string affiliateURLsp { get; set; }
        public ProductImageurl imageURL { get; set; }
        public Sampleimageurl sampleImageURL { get; set; }
        public Samplemovieurl sampleMovieURL { get; set; }
        public Prices prices { get; set; }
        public string date { get; set; }
        public Iteminfo iteminfo { get; set; }
    }

    public class Review
    {
        public int count { get; set; }
        public string average { get; set; }
    }

    public class ProductImageurl
    {
        public string list { get; set; }
        public string small { get; set; }
        public string large { get; set; }
    }

    public class Sampleimageurl
    {
        public Sample_S sample_s { get; set; }
    }

    public class Sample_S
    {
        public string[] image { get; set; }
    }

    public class Samplemovieurl
    {
        public string size_476_306 { get; set; }
        public string size_560_360 { get; set; }
        public string size_644_414 { get; set; }
        public string size_720_480 { get; set; }
        public int pc_flag { get; set; }
        public int sp_flag { get; set; }
    }

    public class Prices
    {
        public string price { get; set; }
        public Deliveries deliveries { get; set; }
    }

    public class Deliveries
    {
        public Delivery[] delivery { get; set; }
    }

    public class Delivery
    {
        public string type { get; set; }
        public string price { get; set; }
    }

    public class Iteminfo
    {
        public Genre[] genre { get; set; }
        public Maker[] maker { get; set; }
        public ProductActress[] actress { get; set; }
        public Director[] director { get; set; }
        public Label[] label { get; set; }
        public Series[] series { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Maker
    {
        public object id { get; set; }
        public string name { get; set; }
    }

    public class ProductActress
    {
        public int id { get; set; }
        public string name { get; set; }
        public string ruby { get; set; }
    }

    public class Director
    {
        public int id { get; set; }
        public string name { get; set; }
        public string ruby { get; set; }
    }

    public class Label
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Series
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
