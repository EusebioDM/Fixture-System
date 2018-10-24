using System;
using System.Collections;
using System.Collections.Generic;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Interfaces
{
    public interface ILoggerServices
    {
        IEnumerable<LogDTO> GetLogs(DateTime start, DateTime end);
    }
}
