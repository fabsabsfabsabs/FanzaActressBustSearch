using FanzaActressSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FanzaActressSearch.ViewModel.SearchText
{
    public static class SearchTextResultExtention
    {
        public static IQueryable<Actress> GetActress(this SearchTextResult result, IQueryable<Actress> actress)
            => result.Type switch
            {
                SearchTextResultType.Bust => result.GetBustActress(actress),
                SearchTextResultType.Hip => result.GetHipActress(actress),
                SearchTextResultType.Waist => result.GetWaistActress(actress),
                SearchTextResultType.Cup => result.GetCupActress(actress),
                _ => actress.Where(x => x.Bust >= 100),
            };

        public static string GetSearchTextDetail(this SearchTextResult result)
        {
            var searchTextDetail = result.Type switch
            {
                SearchTextResultType.Bust => result.Size + "cm",
                SearchTextResultType.Cup => result.Cup + "カップ",
                _ => result.Text,
            };
            searchTextDetail += result.ComparisonOperator switch
            {
                ComparisonOperator.Plus => "以上",
                ComparisonOperator.Minus => "以下",
                _ => "",
            };
            return searchTextDetail;
        }

        public static List<SearchRelationText> GetSearchWordList(this SearchTextResult result, Actress actress)
        {
            var searchWordList = new List<SearchRelationText>();
            if (result.Type == SearchTextResultType.Text)
            {
                if (actress != null)
                {
                    var bust = GetBust(actress.Bust);
                    searchWordList.AddBust(bust);
                }
                else
                {
                    searchWordList.Add("-", SearchRelationTextType.Down);
                    searchWordList.Add("-", SearchRelationTextType.Up);
                    searchWordList.Add("-", SearchRelationTextType.Equal);
                }
            }
            else
            {
                searchWordList = result.Type switch
                {
                    SearchTextResultType.Bust => result.GetBustSearchWordList(),
                    SearchTextResultType.Cup => result.GetCupSearchWordList(),
                    _ => throw new ApplicationException(),
                };
            }
            return searchWordList;
        }

        private static IQueryable<Actress> GetBustActress(this SearchTextResult result, IQueryable<Actress> actress)
             => result.ComparisonOperator switch
             {
                 ComparisonOperator.Equal => actress.Where(x => x.Bust == result.Size),
                 ComparisonOperator.Plus => actress.Where(x => x.Bust >= result.Size),
                 ComparisonOperator.Minus => actress.Where(x => x.Bust <= result.Size),
                 _ => throw new NotImplementedException(),
             };

        private static IQueryable<Actress> GetHipActress(this SearchTextResult result, IQueryable<Actress> actress)
             => result.ComparisonOperator switch
             {
                 ComparisonOperator.Equal => actress.Where(x => x.Hip == result.Size),
                 ComparisonOperator.Plus => actress.Where(x => x.Hip >= result.Size),
                 ComparisonOperator.Minus => actress.Where(x => x.Hip <= result.Size),
                 _ => throw new NotImplementedException(),
             };

        private static IQueryable<Actress> GetWaistActress(this SearchTextResult result, IQueryable<Actress> actress)
             => result.ComparisonOperator switch
             {
                 ComparisonOperator.Equal => actress.Where(x => x.Waist == result.Size),
                 ComparisonOperator.Plus => actress.Where(x => x.Waist >= result.Size),
                 ComparisonOperator.Minus => actress.Where(x => x.Waist <= result.Size),
                 _ => throw new NotImplementedException(),
             };

        private static IQueryable<Actress> GetCupActress(this SearchTextResult result, IQueryable<Actress> actress)
         => result.ComparisonOperator switch
         {
             ComparisonOperator.Equal => actress.Where(x => x.Cup == result.Cup),
             ComparisonOperator.Plus => actress.Where(x => x.Cup.CompareTo(result.Cup) >= 0),
             ComparisonOperator.Minus => actress.Where(x => x.Cup.CompareTo(result.Cup) <= 0),
             _ => throw new NotImplementedException(),
         };

        private static List<SearchRelationText> GetBustSearchWordList(this SearchTextResult result)
        {
            var searchWordList = new List<SearchRelationText>();

            var bust = GetBust(result.Size);
            searchWordList.AddBust(bust);
            return searchWordList;
        }

        private static List<SearchRelationText> GetCupSearchWordList(this SearchTextResult result)
        {
            var searchWordList = new List<SearchRelationText>();

            var cup = GetCup(result.Cup);
            searchWordList.AddCup(cup);
            return searchWordList;
        }

        private static void AddBust(this List<SearchRelationText> searchWordList, int bust)
        {
            searchWordList.Add(bust > 70 ? $"{bust - 1}" : "-", SearchRelationTextType.Down);
            searchWordList.Add(bust < 160 ? $"{bust + 1}" : "-", SearchRelationTextType.Up);
            searchWordList.Add($"{bust}+", SearchRelationTextType.Equal);
        }

        private static void AddCup(this List<SearchRelationText> searchWordList, string cup)
        {
            var c = cup.ToUpper().ToCharArray().First();
            searchWordList.Add(c != 'A' ? $"{(char)(c - 1)}" : "-", SearchRelationTextType.Down);
            searchWordList.Add(c != 'Z' ? $"{(char)(c + 1)}" : "-", SearchRelationTextType.Up);
            searchWordList.Add($"{c}+", SearchRelationTextType.Equal);
        }

        private static void Add(this List<SearchRelationText> searchWordList, string value, SearchRelationTextType type)
            => searchWordList.Add(new SearchRelationText($"{value}", $"{Uri.EscapeDataString(value)}", type));

        private static int GetBust(int size) => size < 70 || size > 160 ? 100 : size;
        private static string GetCup(string cup) => string.IsNullOrEmpty(cup) || cup == "-" ? "F" : cup;
    }
}
