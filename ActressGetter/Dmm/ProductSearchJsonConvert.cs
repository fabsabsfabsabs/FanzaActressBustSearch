using FanzaActressSearch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ActressGetter.Dmm
{
    internal static class ProductSearchJsonConvert
    {
        internal static IEnumerable<Product> ToProductList(this ProductSearchJson productSearch)
            => productSearch.result.items.ToList().Select(product => new Product()
            {
                Id = product.content_id,
                Title = product.title,
                Volume = product.volume.ToInt(),
                ReviewCount = product.review?.count ?? 0,
                ReviewAverage = product.review?.average.ToFloat() ?? 0,
                AffiliateURL = product.affiliateURL,
                Image = product.imageURL != null ? Path.GetFileNameWithoutExtension(product.imageURL.list)[0..^2] : "",
                SampleImage = product.sampleImageURL != null ? Path.GetFileNameWithoutExtension(product.sampleImageURL?.sample_s.image[0])[0..^2] : "",
                SampleImageURLCount = product.sampleImageURL?.sample_s.image.Count() ?? 0,
                SampleMovie = product.sampleMovieURL != null ? product.sampleMovieURL.size_720_480.ToSampleMovie("cid=", "/size=") : "",
                SampleMovieURLCount = product.sampleMovieURL?.pc_flag ?? 0,
                Price = product.prices.price,
                PriceList = string.Join(",", product.prices.deliveries?.delivery.Select(x => $"{x.type}:{x.price}") ?? new List<string>()),
                Date = product.date.ToDateTime(),
                GenreNames = string.Join(",", product.iteminfo?.genre?.Select(x => x.name) ?? new List<string>()),
                SeriesNames = string.Join(",", product.iteminfo?.series?.Select(x => x.name) ?? new List<string>()),
                MakerNames = string.Join(",", product.iteminfo?.maker?.Select(x => x.name) ?? new List<string>()),
                ActressNames = string.Join(",", product.iteminfo?.actress?.Select(x => x.name) ?? new List<string>()),
                LabelNames = string.Join(",", product.iteminfo?.label?.Select(x => x.name) ?? new List<string>()),
                IsSingle = product.iteminfo?.actress.Length == 1,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            });

        private static string ToSampleMovie(this string url, string prefix, string suffix)
            => url != null ? url.Substring(url.IndexOf(prefix) + prefix.Length, url.IndexOf(suffix) - (url.IndexOf(prefix) + prefix.Length)) : "";

        private static DateTime ToDateTime(this object value)
            => DateTime.TryParse(value?.ToString() ?? "", out var result) ? result : new DateTime(1900, 1, 1);

        private static int ToInt(this string value)
            => int.TryParse(value, out var result) ? result : 0;

        private static float ToFloat(this string value)
            => float.TryParse(value, out var result) ? result : 0;
    }
}
