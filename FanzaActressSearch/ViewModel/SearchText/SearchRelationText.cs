namespace FanzaActressSearch.ViewModel.SearchText
{
    public enum SearchRelationTextType
    {
        Equal,
        Up,
        Down,
    }

    public record SearchRelationText(string Link, string Url, SearchRelationTextType Type);
}
