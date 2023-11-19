using Giacom.CallDetails.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Giacom.CallDetails.WebApi.Controllers;

[ApiController]
[Route("api/v1/cdr")]
public class CallDetailsController : ControllerBase
{ 
    public CallDetailsController()
    {
    }

    [HttpPost]
    [Route("upload")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public Task<ActionResult> Upload([FromForm] CsvFile file)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [Route("{reference}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<CallDetailRecordDto>> GetByReference([FromRoute] string reference)
    {
        throw new NotImplementedException();
    }
}