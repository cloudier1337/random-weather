using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AteaWeather.Services.Services;
using AteaWeather.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CallWeatherDataFunction;


[Route("api")]
[ApiController]
public class GetWeatherDataController : ControllerBase
{
    private readonly IBlobService _blobService;
    private readonly ITableService _tableService;
    
    public GetWeatherDataController(IBlobService blobService, ITableService tableService)
    {
        _blobService = blobService;
        _tableService = tableService;
    }
    
    // GET
    [HttpGet]
    [Route("get_logs")]
    public async Task<ActionResult<LogRecordEntityDto>> GetLogs(DateTime fromDateTime, DateTime toDateTime)
    {
        
        try
        {
            // Retrieve logs from the table storage
            List<LogRecordEntityDto> logs = await _tableService.GetLogsAsync(fromDateTime, toDateTime);

            // Return logs as JSON
            return new OkObjectResult(logs);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    // GET
    [HttpGet]
    [Route("get_blob")]
    public async Task<ActionResult<LogRecordEntityDto>> GetBlob(string blobName)
    {
        try
        {
            
            // Retrieve blob content
            string blobContent = await _blobService.GetBlobAsync(blobName);

            // Return blob content as JSON
            return new OkObjectResult(blobContent);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}