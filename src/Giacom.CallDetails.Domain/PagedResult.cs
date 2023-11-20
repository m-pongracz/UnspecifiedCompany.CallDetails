namespace Giacom.CallDetails.Domain;

public class PagedResult<T> : List<T>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    
    public PagedResult(PagingRequest paging, IEnumerable<T> items)
    {
        PageNumber = paging.PageNumber;
        PageSize = paging.PageSize;
        
        AddRange(items);
    }    
}