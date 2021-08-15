using System.Collections.Generic;

namespace FanzaActressSearch.ViewModel.Select
{
    public class BustCup : IViewSelect
    {
        public BustCup()
        {
            for (char i = 'A'; i <= 'Z'; i++)
            {
                Items.Add($"{i}", $"{i}カップ");
            }
            Value = Default;
        }
        public string Id => "BustCup";
        public string Value { get; set; }
        public string Default => "A";
        public string Title => "";
        public Dictionary<string, string> Items { get; } = new Dictionary<string, string>();
    }
}
