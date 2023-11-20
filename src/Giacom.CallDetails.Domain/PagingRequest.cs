namespace Giacom.CallDetails.Domain;

public record PagingRequest(int PageNumber, int PageSize)
{
    public (int skip, int take) GetSkipAndTake()
        => ((PageNumber - 1) * PageSize, PageSize);
}