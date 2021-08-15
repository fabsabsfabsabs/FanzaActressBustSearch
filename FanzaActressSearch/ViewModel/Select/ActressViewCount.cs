using System.Collections.Generic;

namespace FanzaActressSearch.ViewModel.Select
{
    public class ActressViewCount : IViewSelect
    {
        public string Id => "ActressViewCount";
        public string Value { get; set; }
        public string Default => "25";
        public string Title => "表示件数";
        public Dictionary<string, string> Items => new Dictionary<string, string>()
        {
            {"10","10件"},
            {"25","25件"},
            {"50","50件"},
        };
    }
}
