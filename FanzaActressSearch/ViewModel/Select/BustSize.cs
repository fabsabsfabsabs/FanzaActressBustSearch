using System.Collections.Generic;

namespace FanzaActressSearch.ViewModel.Select
{
    public class BustSize : IViewSelect
    {
        public BustSize()
        {
            for (int i = 70; i <= 160; i++)
            {
                Items.Add($"{i}", $"{i}cm");
            }
            Value = Default;
        }
        public string Id => "BustSize";
        public string Value { get; set; }
        public string Default => "70";
        public string Title => "";
        public Dictionary<string, string> Items { get; } = new Dictionary<string, string>();
    }
}
