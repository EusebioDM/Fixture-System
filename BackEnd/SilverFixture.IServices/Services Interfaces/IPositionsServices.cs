using System.Collections.Generic;
using SilverFixture.IServices.DTOs;

namespace SilverFixture.IServices.Services_Interfaces
{
    public interface IPositionsServices
    {
        IReadOnlyDictionary<string, int> GetPositionsTable(string sport);
    }
}