using System;
using System.Collections.Generic;

namespace FanzaActressSearch.ViewModel
{
    public record PaginatedParameter(
        string Url,
        string Result,
        int Page,
        int ViewCount,
        int FirstPage,
        int BeginPage,
        int EndPage,
        int LastPage,
        bool IsBeginPage,
        bool IsEndPage,
        bool IsFirstPage,
        bool IsLastPage,
        bool IsPage);

    public class PaginatedList<T> : List<T>
    {
        public PaginatedParameter Parameter { get; }
        public int Total { get; }

        const int PageRange = 4;
        public PaginatedList(List<T> items, int page, int count, int total, string url)
        {
            if (page < 1) throw new ArgumentOutOfRangeException();
            if (count < 0) throw new ArgumentOutOfRangeException();
            if (total < 0) throw new ArgumentOutOfRangeException();

            Total = total;

            var lastPage = count > 0 ? (total + count - 1) / count : 0;
            var beginPage = lastPage <= PageRange ? 1 : Math.Max(page - PageRange, 1);
            var endPage = lastPage <= PageRange ? lastPage : beginPage + PageRange - 1;
            if (endPage > lastPage)
            {
                endPage = lastPage;
                beginPage = endPage - PageRange + 1;
            }
            var isBeginPage = page > 1;
            var isEndPage = page < lastPage;
            var isFirstPage = beginPage > 1;
            var isLastPage = lastPage <= PageRange ? false : page != lastPage;
            var isPage = beginPage != endPage && total != 0;
            var result = isPage ? $"全{total}件({(page - 1) * count + 1}~{Math.Min(page * count, total)}件)" : $"全{total}件";
            Parameter = new PaginatedParameter(url, result, page, count, 1, beginPage, endPage, lastPage, isBeginPage, isEndPage, isFirstPage, isLastPage, isPage);
            AddRange(items);
        }
    }
}