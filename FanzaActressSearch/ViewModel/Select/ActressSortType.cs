using System.Collections.Generic;

namespace FanzaActressSearch.ViewModel.Select
{
    public class ActressSortType : IViewSelect
    {
        public string Id => "ActressSortType";
        public string Value { get; set; }
        public string Default => "Name";
        public string Title => "並べ替え";
        public Dictionary<string, string> Items => new Dictionary<string, string>()
        {
            {"Name","名前順"},
            {"LastSingle","新作順"},
            {"FirstSingle","デビュー順"},
            {"WorkCounter","作品数順"},
            {"Bust","バスト順"},
        };
    }
}
