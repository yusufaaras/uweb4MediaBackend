namespace uweb4Media.Application.Features.CQRS.Queries.Admin.Video;

public class SearchVideoQuery
{
    public string SearchText { get; set; }
    public SearchVideoQuery(string searchText)
    {
        SearchText = searchText;
    }
}