using System.Collections.Generic;
using SilverFixture.Domain.Fixture;

namespace SilverFixture.Domain.Positions
{
    public interface IPositionGenerator
    {
        IEnumerable<Results> GeneratePositions(IEnumerable<Team> teams);
    }
}