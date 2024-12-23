namespace ElasticSearch.Api.Dtos;

public class BaseFilter
{
    private int _pageIndex = 1;
    private  int _pageSize = 5;

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            _pageSize = value <= 0 ? 5 : value;
        }
    }
    
    public string SearchTerm { get; set; } = string.Empty;
}
