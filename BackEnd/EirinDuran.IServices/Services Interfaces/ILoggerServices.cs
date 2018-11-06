using System;
using System.Collections.Generic;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Services_Interfaces
{
    public interface ILoggerServices
    {
        IEnumerable<LogDTO> GetLogs(DateTime start, DateTime end);
    }
}
