using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ActressGetter.Service
{
    [DataContractAttribute]
    public class AnalyzeImage
    {
        [DataMemberAttribute]
        public Description description { get; set; } = new Description();
        [DataMemberAttribute]
        public Adult adult { get; set; } = new Adult();
        [DataMemberAttribute]
        public string requestId { get; set; } = "";
        [DataMemberAttribute]
        public Metadata metadata { get; set; } = new Metadata();
    }

    [DataContractAttribute]
    public class Description
    {
        [DataMemberAttribute]
        public string[] tags { get; set; } = new List<string>().ToArray();
        [DataMemberAttribute]
        public Caption[] captions { get; set; } = new List<Caption>().ToArray();
    }

    [DataContractAttribute]
    public class Caption
    {
        [DataMemberAttribute]
        public string text { get; set; } = "";
        [DataMemberAttribute]
        public float confidence { get; set; }
    }

    [DataContractAttribute]
    public class Adult
    {
        [DataMemberAttribute]
        public bool isAdultContent { get; set; }
        [DataMemberAttribute]
        public float adultScore { get; set; }
        [DataMemberAttribute]
        public bool isRacyContent { get; set; }
        [DataMemberAttribute]
        public float racyScore { get; set; }
    }

    [DataContractAttribute]
    public class Metadata
    {
        [DataMemberAttribute]
        public int height { get; set; }
        [DataMemberAttribute]
        public int width { get; set; }
        [DataMemberAttribute]
        public string format { get; set; } = "";
    }

}
