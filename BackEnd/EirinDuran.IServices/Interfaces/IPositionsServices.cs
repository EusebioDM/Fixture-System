using System.Collections.Generic;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Interfaces
{
    public interface IPositionsServices
    {
        Dictionary<string, int> GetPositionsTable(SportDTO sportDto);
    }
}