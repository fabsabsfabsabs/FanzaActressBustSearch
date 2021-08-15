using System.Text;

namespace FanzaActressSearch.ViewModel.SearchText
{
    public static class SearchTextExtention
    {
        public static SearchTextResult Analysis(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) searchText = "100+";
            var text = searchText;
            var type = SearchTextResultType.Text;
            var comparison = ComparisonOperator.Equal;
            var size = 0;
            var cup = "";

            //半角小文字変換
            text = text.Normalize(NormalizationForm.FormKC).ToLower();
            char startWord = text[0];
            char endWord = text[^1];
            //1桁目数字で2桁以上の場合はbに補正する。1桁数字は文字。
            if ('0' <= startWord && startWord <= '9' && text.Length >= 2)
            {
                text = text.Insert(0, "b");
                startWord = text[0];
            }

            //最初が半角アルファベット
            if ('a' <= startWord && startWord <= 'z')
            {
                //カップ
                if (text.Length <= 2)
                {
                    cup = startWord.ToString().ToUpper();
                    text = cup;
                    type = SearchTextResultType.Cup;
                }
                //サイズ
                else
                {
                    if (startWord == 'b') type = SearchTextResultType.Bust;
                    var bText = endWord == '+' ? text[1..^1] : text[1..];
                    if (int.TryParse(bText, out size)) text = size.ToString();
                }
                if (endWord == '+')
                {
                    comparison = ComparisonOperator.Plus;
                    text += "+";
                }
            }
            return new SearchTextResult(type, text, size, cup, comparison);
        }
    }
}
