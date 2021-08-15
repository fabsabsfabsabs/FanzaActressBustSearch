using FanzaActressSearch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ActressGetter.Dmm
{
    internal static class ActressSearchJsonConvert
    {
        internal static IEnumerable<Actress> ToActressList(this ActressSearchJson actressSearch)
            => actressSearch.result.actress.ToList().Select(actress => new Actress()
            {
                Id = actress.id.ToInt(),
                Name = actress.name,
                Ruby = actress.ruby,
                Bust = actress.bust.ToInt(),
                Cup = actress.cup.ToStr(),
                Waist = actress.waist.ToInt(),
                Hip = actress.hip.ToInt(),
                Height = actress.height.ToInt(),
                Birthday = actress.birthday.ToDateTime(),
                BloodType = actress.blood_type.ToStr(),
                Hobby = actress.hobby.ToStr(),
                Prefectures = actress.prefectures.ToStr(),
                ImageURL = actress.imageURL == null ? "" : Path.GetFileNameWithoutExtension(actress.imageURL.small.ToStr()),
                DigitalURL = actress.listURL?.digital.ToStr() ?? "",
                MonthlyURL = actress.listURL?.monthly.ToStr() ?? "",
                MonoURL = actress.listURL?.mono.ToStr() ?? "",
                RentalURL = actress.listURL?.rental.ToStr() ?? "",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            });

        private static DateTime ToDateTime(this object value)
            => DateTime.TryParse(value?.ToString() ?? "", out var result) ? result : new DateTime(1900, 1, 1);

        private static string ToStr(this object value)
            => (value?.ToString() ?? "").ToStr();

        private static int ToInt(this string value)
            => int.TryParse(value, out var result) ? result : 0;

        private static string ToStr(this string value)
            => string.IsNullOrEmpty(value) ? "" : value;
    }
}
