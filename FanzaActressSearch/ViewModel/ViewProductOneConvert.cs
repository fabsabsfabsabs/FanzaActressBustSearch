using FanzaActressSearch.Models;

namespace FanzaActressSearch.ViewModel
{
    public static class ViewProductOneConvert
    {
        public static ViewProductOne ToViewProductOne(this Product product)
            => new ViewProductOne()
            {
                Title = product.Title,
                Id = product.Id,
                Url = product.AffiliateURL,
                ImageUrlLarge = product.GetImageUrlLarge(),
                SampleImageURL = product.GetSampleImage(),
                SampleImageURLCount = product.SampleImageURLCount,
                SampleMoviePcURL = product.GetSampleMovie720x480(),
                SampleMovieSpURL = product.GetSampleMovie476x306(),
                Date = product.Date.ToString("yyyy/MM/dd"),
                Volume = product.Volume,
                GenreNames = product.GenreNames.ToStr(),
                ActressNames = product.ActressNames.ToStr(),
                SeriesNames = product.SeriesNames.ToStr(),
                MakerNames = product.MakerNames.ToStr(),
            };
        private static string ToStr(this string value)
            => string.IsNullOrEmpty(value) ? "-" : value.ToString();
    }
}
