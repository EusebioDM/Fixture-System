using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public interface IFixtureGenerator
    {
        ICollection<Encounter> GenerateFixture(IEnumerable<Team> teams, DateTime start);
    }
}
