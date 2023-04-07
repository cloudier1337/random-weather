using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AteaWeather.Services.Services.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AteaWeather.Services.Services;

public class TableService : ITableService
{
    private const string TableName = "logs"; // Table name
    private static CloudTable _table;

    public void Initialize(string connectionString)
    {
        // Create a CloudStorageAccount object and connect to Azure Storage
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

        // Create a CloudTableClient object to interact with the table service
        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

        // Get a reference to the table
        _table = tableClient.GetTableReference(TableName);
    }

    public async Task<List<LogRecordEntityDto>> GetLogsAsync(DateTime from, DateTime to)
    {
        // Create a query that retrieves logs for the specified time period
        TableQuery<LogRecordEntityDto> query = new TableQuery<LogRecordEntityDto>()
            .Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, from),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, to)));

        // Execute the query
        TableContinuationToken token = null;
        var logs = new List<LogRecordEntityDto>();
        do
        {
            var segment = await _table.ExecuteQuerySegmentedAsync(query, token);
            logs.AddRange(segment.Results);
            token = segment.ContinuationToken;
        } while (token != null);

        return logs;
    }
    
    public async Task InsertLog(LogRecordEntityDto record)
    {
        var insertOperation = TableOperation.Insert(record);
        await _table.ExecuteAsync(insertOperation);
    }
}

public class LogRecordEntityDto : TableEntity
{
    public LogRecordEntityDto() { }
    public Guid? BlobRecordId { get; set; }
    public bool Success { get; set; }
}