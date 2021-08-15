using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using FanzaActressSearch.Data;
using FanzaActressSearch.Models;
using FanzaActressSearch.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using FanzaActressSearch.ViewModel.Select;
using FanzaActressSearch.ViewModel.SearchText;

namespace FanzaActressSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly FanzaActressSearchContext _context;
        private DateTime NullDataTime = new DateTime(1900, 1, 1);

        private async Task<SearchTextResult> SetNavBar(string searchText, bool index = false)
        {
            //index以外で未入力の場合はクッキーから取得
            if (string.IsNullOrEmpty(searchText) && !index)
            {
                var cookies = HttpContext.Request.Cookies["SearchText"];
                if (!string.IsNullOrEmpty(cookies)) searchText = cookies;
                ViewData["SearchText"] = searchText;
            }
            var result = SearchTextExtention.Analysis(searchText);
            var actress = result.Type == SearchTextResultType.Text ? await _context.Actress.AsNoTracking().Where(x => x.Name.Contains(result.Text)).FirstOrDefaultAsync() : null;
            var bustSize = new BustSize();
            var bustCup = new BustCup();
            if (result.Type == SearchTextResultType.Bust)
            {
                var hit = await _context.CupBust.AsNoTracking().Where(x => x.Bust >= result.Size).FirstOrDefaultAsync();
                bustSize.Value = result.Size.ToString();
                bustCup.Value = hit?.Cup;
            }
            else if (result.Type == SearchTextResultType.Cup)
            {
                var hit = await _context.CupBust.AsNoTracking().Where(x => x.Cup == result.Cup).FirstOrDefaultAsync();
                bustSize.Value = hit?.Bust.ToString();
                bustCup.Value = result.Cup;
            }
            else if (actress != null)
            {
                bustSize.Value = actress.Bust.ToString();
                bustCup.Value = actress.Cup;
            }
            ViewData["SearchWordList"] = result.GetSearchWordList(actress);
            ViewData["SearchTextDetail"] = result.GetSearchTextDetail();
            ViewData["BustSize"] = bustSize;
            ViewData["BustCup"] = bustCup;
            return result;
        }

        public HomeController(FanzaActressSearchContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchText = "", int page = 0, string count = "")
        {
            ViewData["OgUrl"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            ViewData["Canonical"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            page = Math.Max(page, 1);

            var updateDate = await _context.ActressScrapingResult.AsNoTracking().OrderByDescending(x => x.CreateDate).FirstAsync();
            ViewData["UpdateDate"] = $"データ取得日:{updateDate.CreateDate:yy/MM/dd}";

            var actressViewCount = new ActressViewCount();
            var countNum = int.Parse(actressViewCount.SetValue(count, Request.Cookies, Response.Cookies));
            ViewData["ActressViewCount"] = actressViewCount;

            var actressViewType = new ActressViewType();
            actressViewType.SetValue("", Request.Cookies, Response.Cookies);
            ViewData["ActressViewType"] = actressViewType;

            var actressSortType = new ActressSortType();
            actressSortType.SetValue("", Request.Cookies, Response.Cookies);
            ViewData["ActressSortType"] = actressSortType;

            ViewData["ActressViewArrow"] = Request.Cookies["ActressViewArrow"] == "up" ? "up" : "down";
            var up = ViewData["ActressViewArrow"].ToString() == "up";

            searchText ??= "";
            SearchTextResult result;
            HttpContext.Response.Cookies.Append("SearchText", searchText);
            ViewData["SearchText"] = searchText;
            result = await SetNavBar(searchText, true);

            var actress = _context.Actress.AsNoTracking();
            actress = (result.Type == SearchTextResultType.Text) ? actress.Where(x => x.Name.Contains(result.Text)) : result.GetActress(actress);
            actress = actress.Where(x => x.Name != "このIDは使われておりません").Where(x => x.Name != "こもIDは使われておりません").Where(x => x.Name != "このID使われておりません").Where(x => x.Name != "検証花");
            var actressCount = await actress.CountAsync();
            actress = actressSortType.Value switch
            {
                "WorkCounter" => up ? actress.OrderBy(x => x.WorkCounter) : actress.OrderByDescending(x => x.WorkCounter),
                "LastSingle" => up ? actress.OrderBy(x => x.LastSingle) : actress.OrderByDescending(x => x.LastSingle),
                "Bust" => up ? actress.OrderBy(x => x.Bust) : actress.OrderByDescending(x => x.Bust),
                "FirstSingle" => up ? actress.Where(x => x.FirstSingle != NullDataTime).OrderBy(x => x.FirstSingle) : actress.Where(x => x.FirstSingle != NullDataTime).OrderByDescending(x => x.FirstSingle),
                _ => up ? actress.OrderByDescending(x => x.Ruby) : actress.OrderBy(x => x.Ruby),
            };
            var actressList = (await actress.Skip((page - 1) * countNum).Take(countNum).ToListAsync()).ToViewActress(Request.Cookies["Verification"] == "Ok").Distinct().ToList();
            _context.Search.Add(new Search() { Text = result.Text, Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(), CreateDate = DateTime.Now });
            _context.SaveChanges();

            var cookies = HttpContext.Request.Cookies["History"];
            if (string.IsNullOrEmpty(cookies))
            {
                HttpContext.Response.Cookies.Append("History", result.Text);
            }
            else
            {
                if(!cookies.Split(",").Any(x => x == result.Text))
                {
                    HttpContext.Response.Cookies.Append("History", $"{cookies},{result.Text}");
                }
            }
            ViewData["History"] = HttpContext.Request.Cookies["History"];

            return View(new PaginatedList<ViewActress>(actressList, page, countNum, actressCount, $"?searchText={Uri.EscapeDataString(result.Text)}&"));
        }

        [Route("/home/actress")]
        [Route("/actress/{id}")]
        [Route("/actress/{id}/{name}")]
        public async Task<IActionResult> Actress(int id, int page = 0, string count = "")
        {
            if (!IsVerification()) return Redirect($"/home/verification?url=/actress/{id}");

            await SetNavBar("");
            var actress = _context.Actress.AsNoTracking().Where(x => x.Id == id);
            if (!actress.Any()) return Redirect("/home/error");

            ViewData["OgUrl"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/actress/{id}";
            ViewData["Canonical"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/actress/{id}";
            page = Math.Max(page, 1);
            var productViewCount = new ProductViewCount();
            var countNum = int.Parse(productViewCount.SetValue(count, Request.Cookies, Response.Cookies));
            ViewData["ProductViewCount"] = productViewCount;
            var onlySingle = Request.Cookies["OnlySingle"] == null || string.Compare(Request.Cookies["OnlySingle"], "true", true) == 0;
            var onlyWithMovie = Request.Cookies["OnlyWithMovie"] != null && string.Compare(Request.Cookies["OnlyWithMovie"], "true", true) == 0;
            ViewData["OnlySingle"] = onlySingle ? "checked" : "";
            ViewData["OnlyWithMovie"] = onlyWithMovie ? "checked" : "";

            var product = _context.ActressProduct.AsNoTracking()
                .Join(actress, actressProduct => actressProduct.ActressID, actress => actress.Id, (actressProduct, actress) => actressProduct)
                .Join(_context.Product.AsNoTracking(), actressProduct => actressProduct.ProductID, product => product.Id, (actressProduct, product) => product)
                .Where(x => !onlySingle || (onlySingle && x.IsSingle))
                .Where(x => !onlyWithMovie || (onlyWithMovie && x.SampleMovieURLCount != 0));
            var productList = product.OrderBy(x => x.Date).Skip((page - 1) * countNum).Take(countNum);
            var productCount = await product.CountAsync();
            var one = (await actress.SingleOrDefaultAsync()).ToViewActressOne(await productList.ToListAsync(), page, countNum, productCount);
            one.SetPrevNextMovie();
            return View(one);
        }

        [Route("/home/product")]
        [Route("/actress/{actressId}/product/{id}")]
        [Route("/actress/{actressId}/product/{id}/{name}")]
        public async Task<IActionResult> Product(string id, int actressId)
        {
            if (!IsVerification()) return Redirect($"/home/verification?url=/actress/{actressId}/product/{id}");

            await SetNavBar("");
            var product = await _context.Product.AsNoTracking().Where(x => x.Id == id).SingleOrDefaultAsync();
            if (product == null) return Redirect("/home/error");
            ViewData["OgUrl"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/actress/{actressId}/product/{id}";
            ViewData["Canonical"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/actress/{actressId}/product/{id}";

            var actress = new Actress();
            if(actressId == 0)
            {
                var firstActress = product.ActressNames.Split(',')[0];
                actress = await _context.Actress.AsNoTracking().FirstOrDefaultAsync(x => x.Name == firstActress) ?? new Actress();
            }
            else
            {
                actress = await _context.Actress.AsNoTracking().SingleOrDefaultAsync(x => x.Id == actressId);
            }
            ViewData["ActressId"] = actress.Id;
            ViewData["ActressName"] = actress.Name;

            return View(product.ToViewProductOne());
        }

        public async Task<IActionResult> Verification(string url)
        {
            await SetNavBar("");
            ViewData["Url"] = url;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            await SetNavBar("");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<IActionResult> Twitter()
        //{
        //    ViewData["OgUrl"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/twitter";
        //    var updateDate = await _context.ActressScrapingResult.AsNoTracking().OrderByDescending(x => x.CreateDate).FirstAsync();
        //    ViewData["UpdateDate"] = $"データ取得日:{updateDate.CreateDate:yy/MM/dd}";
        //    ViewData["Sns"] = "Twitter";
        //    SetNavBar("", false);
        //    var actressList = await _context.Actress.AsNoTracking().Where(x => x.TwitterName != "").OrderByDescending(x => x.Bust).ToListAsync();
        //    var viewActressList = actressList.ToViewActress(IsVerification()).Distinct().ToList();
        //    return View(new PaginatedList<ViewActress>(viewActressList, 1, viewActressList.Count, viewActressList.Count, $""));
        //}

        //public async Task<IActionResult> Instagram()
        //{
        //    ViewData["OgUrl"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/instagram";
        //    var updateDate = await _context.ActressScrapingResult.AsNoTracking().OrderByDescending(x => x.CreateDate).FirstAsync();
        //    ViewData["UpdateDate"] = $"データ取得日:{updateDate.CreateDate:yy/MM/dd}";
        //    ViewData["Sns"] = "Instagram";
        //    SetNavBar("", false);
        //    var actressList = await _context.Actress.AsNoTracking().Where(x => x.InstagramName != "").OrderByDescending(x => x.Bust).ToListAsync();
        //    var viewActressList = actressList.ToViewActress(IsVerification()).Distinct().ToList();
        //    return View(new PaginatedList<ViewActress>(viewActressList, 1, viewActressList.Count, viewActressList.Count, $""));
        //}

        private bool IsVerification()
           => Request.Cookies["Verification"] == null || Request.Cookies["Verification"] == "Ok";
    }
}
