using System;
using System.Collections.Generic;
using System.Text;

namespace SilverFixture.Domain.Fixture
{
    public interface IFixtureGenerator
    {
        ICollection<Encounter> GenerateFixture(IEnumerable<Team> teams, DateTime start);
    }
}
