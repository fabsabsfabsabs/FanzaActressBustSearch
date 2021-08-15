namespace FanzaActressSearch.ViewModel.SearchText
{
    public enum SearchTextResultType
    {
        Text,
        Bust,
        Waist,
        Hip,
        Cup,
    }

    public enum ComparisonOperator
    {
        Equal,
        Plus,
        Minus,
    }

    public record SearchTextResult(SearchTextResultType Type, string Text, int Size, string Cup, ComparisonOperator ComparisonOperator);
}
