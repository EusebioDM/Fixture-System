using System;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.Domain;
using EirinDuran.Domain.Fixture;

namespace EirinDuran.FixtureGenerators.AllOnce
{
    public class AllOnce : IFixtureGenerator
    {
        private Sport sport;

        public ICollection<Encounter> GenerateFixture(IEnumerable<Team> teams, DateTime start)
        {
            List<Encounter> encounters = new List<Encounter>();
            List<Team> teamList = teams.ToList();
            sport = teams.First().Sport;

            bool areRepeatedTeams = teamList.GroupBy(n => n).Any(t => t.Count() > 1);

            if (areRepeatedTeams)
            {
                throw new DomainException(teams, "there are repeated teams");
            }

            if (teamList.Count % 2 != 0)
            {
                teamList.RemoveAt(teamList.Count - 1);
            }

            GenerateAllOnceFixture(encounters, teamList, start);

            return encounters;
        }

        private void GenerateAllOnceFixture(List<Encounter> encounters, List<Team> teamList, DateTime start)
        {
            int amountTeams = teamList.Count;
            int middleTeams = amountTeams / 2;

            Team[] local = ConvertCollectionOfTeamToVector(teamList, 0, middleTeams);
            Team[] visitant = ConvertCollectionOfTeamToVector(teamList, middleTeams, amountTeams);

            for (int i = 0; i < middleTeams; i++)
            {
                List<Team> teamsInEncounter = new List<Team>()
                {
                    local[i],
                    visitant[i]
                };
                encounters.Add(new Encounter(sport, teamsInEncounter, start));
            }
        }

        private Team[] ConvertCollectionOfTeamToVector(IEnumerable<Team> teams, int since, int until)
        {
            List<Team> teamsList = teams.ToList();
            Team[] teamsVector = new Team[until - since];

            for (int i = since; i < until; i++)
            {
                teamsVector[i - since] = teamsList[i];
            }

            return teamsVector;
        }
    }
}