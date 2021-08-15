using System.Collections.Generic;

namespace FanzaActressSearch.ViewModel.Select
{
    public class ProductViewCount : IViewSelect
    {
        public string Id => "ProductViewCount";
        public string Value { get; set; }
        public string Default => "12";
        public string Title => "表示件数";
        public Dictionary<string, string> Items => new Dictionary<string, string>()
        {
            {"12","12件"},
            {"24","24件"},
            {"36","36件"},
        };
    }
}
