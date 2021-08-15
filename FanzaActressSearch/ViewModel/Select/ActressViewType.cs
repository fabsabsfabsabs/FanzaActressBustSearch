using System.Collections.Generic;

namespace FanzaActressSearch.ViewModel.Select
{
    public class ActressViewType : IViewSelect
    {
        public string Id => "ActressViewType";
        public string Value { get; set; }
        public string Default => "BustupM";
        public string Title => "表示形式";
        public Dictionary<string, string> Items => new Dictionary<string, string>()
        {
            {"BustupS","画像小"},
            {"BustupM","画像大"},
        };
    }
}
