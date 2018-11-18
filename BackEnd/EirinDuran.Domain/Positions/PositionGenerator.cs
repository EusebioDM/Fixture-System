using System.Collections.Generic;
using EirinDuran.Domain.Fixture;

namespace EirinDuran.Domain.Positions
{
    public interface IPositionGenerator
    {
        IEnumerable<Results> GeneratePositions(IEnumerable<Team> teams);
    }
}