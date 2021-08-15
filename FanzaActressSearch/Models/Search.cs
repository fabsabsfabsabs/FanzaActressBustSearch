using System;

namespace FanzaActressSearch.Models
{
    public record Search
    {
        public int SearchID { get; set; }
        public string Text { get; set; }
        public string Ip { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
