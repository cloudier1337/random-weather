using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AteaWeather.Services.Services.Interfaces;

public interface ITableService
{
    void Initialize(string connectionString);
    Task<List<LogRecordEntityDto>> GetLogsAsync(DateTime from, DateTime to);
    Task InsertLog(LogRecordEntityDto record);
}