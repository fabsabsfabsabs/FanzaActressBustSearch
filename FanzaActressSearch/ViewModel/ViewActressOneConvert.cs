using FanzaActressSearch.Models;
using System.Collections.Generic;
using System.Linq;

namespace FanzaActressSearch.ViewModel
{
    public static class ViewActressOneConvert
    {
        public static ViewActressOne ToViewActressOne(this Actress actress, IReadOnlyCollection<Product> productList, int page, int count, int total)
            => new ViewActressOne()
            {
                Id = actress.Id,
                Name = actress.Name,
                Ruby = actress.Ruby,
                Bust = actress.Bust == 0 ? "-" : actress.Bust.ToString(),
                Waist = actress.Waist == 0 ? "-" : actress.Waist.ToString(),
                Hip = actress.Hip == 0 ? "-" : actress.Hip.ToString(),
                Cup = actress.Cup.ToStr(),
                Birthday = actress.Birthday.ToString("yyyy/MM/dd") != "1900/01/01" ? actress.Birthday.ToString("yyyy/MM/dd") : "-",
                BloodType = actress.BloodType.ToStr(),
                Hobby = actress.Hobby.ToStr(),
                Prefectures = actress.Prefectures.ToStr(),
                DigitalURL = actress.DigitalURL,
                MonthlyURL = actress.MonthlyURL,
                MonoURL = actress.MonoURL,
                RentalURL = actress.RentalURL,
                ImageURL = actress.GetImageUrl(true),
                FirstSinglePeriod = (actress.FirstSingle != actress.LastSingle) ? $"{actress.FirstSingle:yyyy/MM/dd}" : "-",
                LastSinglePeriod = (actress.FirstSingle != actress.LastSingle) ? $"{actress.LastSingle:yyyy/MM/dd}" : "-",
                WorkCounter = actress.WorkCounter,
                Product = productList.Count == 0 ? "0" : $"{productList.Count} ({productList.Min(x => x.Date):yyyy/MM/dd}～{productList.Max(x => x.Date):yyyy/MM/dd})",
                Wiki = string.IsNullOrEmpty(actress.Wiki) ? "-" : actress.Wiki,
                TwitterName = string.IsNullOrEmpty(actress.TwitterName) ? "-" : actress.TwitterName,
                InstagramName = string.IsNullOrEmpty(actress.InstagramName) ? "-" : actress.InstagramName,
                ViewProductList = new PaginatedList<ViewProduct>(productList.Select(x => x.ToViewProduct()).ToList(), page, count, total, $"/actress/{actress.Id}?"),
            };

        public static void SetPrevNextMovie(this ViewActressOne one)
        {
            var movieIndex = 1;
            var movieCount = one.ViewProductList.Count(p => !string.IsNullOrEmpty(p.SampleMovieSURL));

            for (int i = 0; i < one.ViewProductList.Count; i++)
            {
                var product = one.ViewProductList[i];
                if (string.IsNullOrEmpty(product.SampleMovieSURL)) continue;

                product.HasMovieNextId = one.ViewProductList.Skip(i + 1).FirstOrDefault(p => !string.IsNullOrEmpty(p.SampleMovieSURL))?.Id ?? "";
                product.HasMoviePrevId = one.ViewProductList.Take(i).LastOrDefault(p => !string.IsNullOrEmpty(p.SampleMovieSURL))?.Id ?? "";
                product.HasMovieNumber = $"{movieIndex++}/{movieCount}";
            }
            var hasMovieFirstProduct = one.ViewProductList.FirstOrDefault(p => !string.IsNullOrEmpty(p.SampleMovieSURL));
            var hasMovieLastProduct = one.ViewProductList.LastOrDefault(p => !string.IsNullOrEmpty(p.SampleMovieSURL));
            if (hasMovieFirstProduct != null && hasMovieLastProduct != null)
            {
                hasMovieFirstProduct.HasMoviePrevId = hasMovieLastProduct.Id;
                hasMovieLastProduct.HasMovieNextId = hasMovieFirstProduct.Id;
            }
        }

        private static string ToStr(this string value)
            => string.IsNullOrEmpty(value) ? "-" : value.ToString();
    }
}
