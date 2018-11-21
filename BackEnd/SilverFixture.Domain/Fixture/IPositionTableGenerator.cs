using System.Collections.Generic;
using PositionsTable = System.Collections.Generic.Dictionary<string, int>;

namespace SilverFixture.Domain.Fixture
{
    public interface IPositionTableGenerator
    {
        PositionsTable GetPositionTable(IEnumerable<Encounter> encounters);
    }
}