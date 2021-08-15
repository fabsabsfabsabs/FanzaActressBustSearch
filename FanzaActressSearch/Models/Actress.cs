using System;
using System.ComponentModel.DataAnnotations;

namespace FanzaActressSearch.Models
{
    public record Actress
    {
        public int ActressID { get; set; }
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Ruby { get; set; }
        public int Bust { get; set; }
        [StringLength(1)]
        public string Cup { get; set; }
        public int Waist { get; set; }
        public int Hip { get; set; }
        public int Height { get; set; }
        public DateTime Birthday { get; set; }
        [StringLength(2)]
        public string BloodType { get; set; }
        public string Hobby { get; set; }
        [StringLength(4)]
        public string Prefectures { get; set; }
        public string ImageURL { get; set; }
        public string DigitalURL { get; set; }
        public string MonthlyURL { get; set; }
        public string MonoURL { get; set; }
        public string RentalURL { get; set; }
        public int LoveCounter { get; set; }
        public int WorkCounter { get; set; }
        public string Wiki { get; set; } = "";
        public string TwitterName { get; set; } = "";
        public string InstagramName { get; set; } = "";
        public bool IsAdultImage { get; set; }
        public DateTime FirstSingle { get; set; }
        public DateTime LastSingle { get; set; }
        public DateTime UpdateDate { get; set; }   //更新日
        public DateTime CreateDate { get; set; }    //作成日
    }

    public static class ActressImageGetter
    {
        private static readonly string ImageUrl = "https://pics.dmm.co.jp/mono/actjpgs/";
        private static readonly string NoImageUrl = "https://pics.dmm.com/mono/movie/n/now_printing/now_printing.jpg";
        public static string GetThumbnailImageUrl(this Actress actress, bool verification)
        {
            if (verification == false && actress.IsAdultImage == true) return NoImageUrl;
            return string.IsNullOrEmpty(actress.ImageURL) ? NoImageUrl : $"{ImageUrl}thumbnail/{actress.ImageURL}.jpg";
        }

        public static string GetImageUrl(this Actress actress, bool verification)
        {
            if (verification == false && actress.IsAdultImage == true) return NoImageUrl;
            return string.IsNullOrEmpty(actress.ImageURL) ? NoImageUrl : $"{ImageUrl}{actress.ImageURL}.jpg";
        }
    }
}
