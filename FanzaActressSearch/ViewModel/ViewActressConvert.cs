using FanzaActressSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FanzaActressSearch.ViewModel
{
    public static class ViewActressConvert
    {
        public static IEnumerable<ViewActress> ToViewActress(this IEnumerable<Actress> actress, bool verification)
            => actress.Select(x => new ViewActress()
            {
                Id = x.Id,
                Name = x.Name,
                Ruby = x.Ruby,
                Bust = x.Bust == 0 ? "-" : x.Bust.ToString(),
                Hip = x.Hip == 0 ? "-" : x.Hip.ToString(),
                Waist = x.Waist == 0 ? "-" : x.Waist.ToString(),
                Cup = string.IsNullOrEmpty(x.Cup) ? "-" : x.Cup.ToString(),
                DigitalURL = x.DigitalURL,
                ImageSURL = x.GetThumbnailImageUrl(verification),
                ImageMURL = x.GetImageUrl(verification),
                IsNew = DateTime.Now < x.LastSingle,
                IsHeart = false,
                FirstSinglePeriod = (x.FirstSingle != x.LastSingle) ? $"{x.FirstSingle:yyyy/MM/dd}" : "-",
                LastSinglePeriod = (x.FirstSingle != x.LastSingle) ? $"{x.LastSingle:yyyy/MM/dd}" : "-",
                TwitterName = x.TwitterName,
                InstagramName = x.InstagramName,
                WorkCounter = x.WorkCounter,
            });
    }
}
