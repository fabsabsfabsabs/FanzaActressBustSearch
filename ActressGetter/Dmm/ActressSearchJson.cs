namespace ActressGetter.Dmm
{
    public class ActressSearchJson
    {
        public Request request { get; set; }
        public Result result { get; set; }
    }

    public class Request
    {
        public Parameters parameters { get; set; }
    }

    public class Parameters
    {
        public string api_id { get; set; }
        public string affiliate_id { get; set; }
        public string actress_id { get; set; }
    }

    public class Result
    {
        public string status { get; set; }
        public int result_count { get; set; }
        public string total_count { get; set; }
        public int first_position { get; set; }
        public ActressData[] actress { get; set; }
    }

    public class ActressData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string ruby { get; set; }
        public string bust { get; set; }
        public string cup { get; set; }
        public string waist { get; set; }
        public string hip { get; set; }
        public string height { get; set; }
        public object birthday { get; set; }
        public object blood_type { get; set; }
        public object hobby { get; set; }
        public object prefectures { get; set; }
        public Imageurl imageURL { get; set; }
        public Listurl listURL { get; set; }
    }

    public class Imageurl
    {
        public string small { get; set; }
        public string large { get; set; }
    }

    public class Listurl
    {
        public string digital { get; set; }
        public string monthly { get; set; }
        public string mono { get; set; }
        public string rental { get; set; }
    }

}
