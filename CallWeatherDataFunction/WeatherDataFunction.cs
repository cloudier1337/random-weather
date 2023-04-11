using System;
using System.Net.Http;
using System.Threading.Tasks;
using AteaWeather.Services.Services;
using AteaWeather.Services.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CallWeatherDataFunction
{
    public class WeatherDataFunction
    {
        private readonly IBlobService _blobService;
        private readonly ITableService _tableService;

        public WeatherDataFunction(IBlobService blobService, ITableService tableService)
        {
            _blobService = blobService;
            _tableService = tableService;
        }

        [Function("WeatherDataFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {

            _blobService.Initialize();
            _tableService.Initialize();
            
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string apiAddress = "https://api.publicapis.org/random?auth=null";
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(apiAddress);

            try
            {
                HttpResponseMessage httpResponse = await httpClient.PostAsync("", null);

                if (httpResponse.IsSuccessStatusCode)
                {
                    log.LogInformation("Success - log failure to table and store response to blob");
                    //store to table
                    Guid blobId = Guid.NewGuid();
                    
                    LogRecordEntityDto logRecord = new LogRecordEntityDto()
                    {
                        Timestamp = DateTime.Now,
                        Success = true,
                        BlobRecordId = blobId
                    };
                    await _tableService.InsertLog(logRecord);
                    
                    
                    // store to blob
                    var blobName = blobId.ToString();
                    var response = await httpResponse.Content.ReadAsStringAsync();
                    
                    await _blobService.StoreToBlob(blobName, response);
                }
                else
                {
                    log.LogInformation("Failure - store failure to table");
                    
                    // store to table
                    LogRecordEntityDto logRecord = new LogRecordEntityDto()
                    {
                        Timestamp = DateTime.Now,
                        Success = false,
                        BlobRecordId = null
                    }; 
                    await _tableService.InsertLog(logRecord);
                    
                    var error = httpResponse.Content.ReadAsStringAsync();
                    log.LogError($"WeatherDataFunction failed. HttpResponse: {error}");
                }
            }
            catch (Exception e)
            {
                string errorMessage = $"Error from {apiAddress} : {e.Message}";
                log.LogError(errorMessage);
                throw new Exception(errorMessage, e);
            }
        }
    }
}