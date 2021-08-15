using Microsoft.AspNetCore.Http;

namespace FanzaActressSearch.ViewModel.Select
{
    public static class ViewSelectExtention
    {
        public static string SetValue(this IViewSelect select, string value, IRequestCookieCollection getCookies, IResponseCookies setcookies)
        {
            if (!string.IsNullOrEmpty(value))
            {
                setcookies.Append(select.Id, value);
                select.Value = value;
            }
            else
            {
                select.Value = string.IsNullOrEmpty(getCookies[select.Id]) ? select.Default : getCookies[select.Id];
            }
            if (!select.Items.ContainsKey(select.Value)) select.Value = select.Default;
            return select.Value;
        }
    }
}
