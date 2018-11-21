using System;
using System.Collections.Generic;
using SilverFixture.IServices.DTOs;

namespace SilverFixture.IServices.Services_Interfaces
{
    public interface ILoggerServices
    {
        IEnumerable<LogDTO> GetLogs(DateTime start, DateTime end);
    }
}
