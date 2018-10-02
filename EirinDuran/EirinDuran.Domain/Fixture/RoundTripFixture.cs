using System;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.Domain.Fixture
{
    public class RoundTripFixture : IFixtureGenerator
    {
        private Sport sport;

        public RoundTripFixture(Sport sport)
        {
            this.sport = sport;
        }

        public ICollection<Encounter> GenerateFixture(IEnumerable<Team> teams, DateTime start)
        {
            List<Encounter> encounters = new List<Encounter>();
            List<Team> teamList = teams.ToList();

            return encounters;
        }
    }
}