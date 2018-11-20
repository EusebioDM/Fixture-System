using System.Collections.Generic;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Services_Interfaces
{
    public interface IPositionsServices
    {
        IReadOnlyDictionary<string, int> GetPositionsTable(string sport);
    }
}