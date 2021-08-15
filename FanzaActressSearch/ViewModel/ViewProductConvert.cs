using FanzaActressSearch.Models;

namespace FanzaActressSearch.ViewModel
{
    public static class ViewProductConvert
    {
        public static ViewProduct ToViewProduct(this Product product)
            => new ViewProduct()
            {
                Title = product.Title,
                Id = product.Id,
                Url = product.AffiliateURL,
                ImageUrl = product.GetImageUrl(),
                ImageUrlSmall = product.GetImageUrlSmall(),
                ImageUrlLarge = product.GetImageUrlLarge(),
                SampleImageURL = product.GetSampleImage(),
                SampleImageURLCount = product.SampleImageURLCount,
                SampleMovieLURL = product.GetSampleMovie720x480(),
                SampleMovieMURL = product.GetSampleMovie560x360(),
                SampleMovieSURL = product.GetSampleMovie476x306(),
                Date = product.Date.ToString("yyyy/MM/dd"),
                Volume = product.Volume,
            };
    }
}
