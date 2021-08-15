using System;
using System.Collections.Generic;

namespace FanzaActressSearch.Models
{
    public record Product
    {
        public int ProductID { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public int Volume { get; set; }
        public int ReviewCount { get; set; }
        public float ReviewAverage { get; set; }
        public string AffiliateURL { get; set; }
        public string Image { get; set; }
        public string SampleImage { get; set; }
        public int SampleImageURLCount { get; set; }
        public string SampleMovie { get; set; }
        public int SampleMovieURLCount { get; set; }
        public string Price { get; set; }
        public string PriceList { get; set; }
        public DateTime Date { get; set; }
        public string GenreNames { get; set; }
        public string SeriesNames { get; set; }
        public string MakerNames { get; set; }
        public string ActressNames { get; set; }
        public string LabelNames { get; set; }
        public bool IsSingle { get; set; }
        public DateTime UpdateDate { get; set; }   //更新日
        public DateTime CreateDate { get; set; }    //作成日
    }

    public static class ProductMediaGetter
    {
        private static readonly string ImageUrl = "https://pics.dmm.co.jp/digital/video";
        private static readonly string MovieUrl = "https://www.dmm.co.jp/litevideo";
        public static string GetImageUrl(this Product product) => $"{ImageUrl}/{product.Image}/{product.Image}pt.jpg";
        public static string GetImageUrlSmall(this Product product) => $"{ImageUrl}/{product.Image}/{product.Image}ps.jpg";
        public static string GetImageUrlLarge(this Product product) => $"{ImageUrl}/{product.Image}/{product.Image}pl.jpg";
        public static string GetSampleImage(this Product product) => product.SampleImageURLCount != 0 ? $"{ImageUrl}/{product.SampleImage}/{product.SampleImage}-" : "";
        public static string GetSampleMovie476x306(this Product product) => product.SampleMovieURLCount != 0 ? $"{MovieUrl}/-/part/=/cid={product.SampleMovie}/size=476_306/" : "";
        public static string GetSampleMovie560x360(this Product product) => product.SampleMovieURLCount != 0 ? $"{MovieUrl}/-/part/=/cid={product.SampleMovie}/size=560_360/" : "";
        public static string GetSampleMovie644x414(this Product product) => product.SampleMovieURLCount != 0 ? $"{MovieUrl}/-/part/=/cid={product.SampleMovie}/size=644_414/" : "";
        public static string GetSampleMovie720x480(this Product product) => product.SampleMovieURLCount != 0 ? $"{MovieUrl}/-/part/=/cid={product.SampleMovie}/size=720_480/" : "";
    }

    public class ProductComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product lhs, Product rhs)
        {
            return lhs.Id == rhs.Id;
        }

        public int GetHashCode(Product product)
        {
            return product.Id.GetHashCode();
        }
    }
}


