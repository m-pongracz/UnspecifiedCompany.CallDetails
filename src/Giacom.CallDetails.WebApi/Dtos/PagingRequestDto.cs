using Giacom.CallDetails.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Giacom.CallDetails.WebApi.Dtos;

public class PagingRequestDto
{
    [FromQuery] 
    public int PageNumber { get; set; } = 1;

    [FromQuery] 
    public int PageSize { get; set; } = 100;

    public PagingRequestDto()
    {
        
    }
    
    public PagingRequest GetPagingRequest()
    {
        return new PagingRequest(PageNumber, PageSize);
    }
}