using Giacom.CallDetails.Application.CallDetails;
using Giacom.CallDetails.Domain.CallDetails;
using Giacom.CallDetails.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Giacom.CallDetails.WebApi.Controllers;

[ApiController]
[Route("api/v1/cdr")]
public class CallDetailsController : ControllerBase
{ 
    private readonly ICallDetailService _callDetailService;

    public CallDetailsController(ICallDetailService callDetailService)
    {
        _callDetailService = callDetailService;
    }

    [HttpPost]
    [Route("upload")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public async Task<ActionResult> Upload([FromForm] CsvFile file)
    {
        await _callDetailService.BulkInsertAsync<CallDetailRecordDto>(file.FormFile.OpenReadStream(),
            dto => dto.GetCallDetail());
        return Ok();
    }

    [HttpGet]
    [Route("{reference}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CallDetailRecordDto>> GetByReference([FromRoute] string reference)
    {
        var callDetail = await _callDetailService.FindAsync(reference);
        return Ok(new CallDetailRecordDto(callDetail));
    }
    
    [HttpGet]
    [Route("count-and-duration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ActionResult<CallDetailCountAndDurationDto>> GetCountAndDuration([FromQuery] DateTime from,
        [FromQuery] DateTime to, [FromQuery] CallType? type)
    {
        throw new NotImplementedException();
    }
}