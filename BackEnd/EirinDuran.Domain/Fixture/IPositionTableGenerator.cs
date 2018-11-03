using System.Collections.Generic;
using PositionsTable = System.Collections.Generic.Dictionary<string, int>;

namespace EirinDuran.Domain.Fixture
{
    public interface IPositionTableGenerator
    {
        PositionsTable GetPositionTable(IEnumerable<Encounter> encounters);
    }
}