using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class LeagueFixture : IFixtureGenerator
    {
        private Sport sport;

        public LeagueFixture(Sport sport)
        {
            this.sport = sport;
        }

        public string Description => throw new NotImplementedException();

        public ICollection<Encounter> GenerateFixture(IEnumerable<Team> teams, DateTime start, DateTime end)
        {
            ControlValidParams(teams, start, end);

            List<Encounter> encounters = new List<Encounter>();
            List<Team> teamList = teams.ToList();

            GenerateLeagueFixture(encounters, teamList, start, end);

            return encounters;
        }

        private void ControlValidParams(IEnumerable<Team> teams, DateTime start, DateTime end)
        {
            int amountTeams = teams.ToList().Count;

            if (amountTeams % 2 != 0)
            {
                throw new InvalidNumberOfTeamsException();
            }
            else if(start > end)
            {
                throw new OutdatedDatesException();
            }
        }

        private void GenerateLeagueFixture(List<Encounter> encounters, List<Team> teamList, DateTime start, DateTime end)
        {
            Team[] teamsVector = ConvertCollectionOfTeamToVector(teamList);
            int amountTeams = teamsVector.Length;
            Encounter encounter;

            for (int i = 0; i < amountTeams; i++)
            {
                List<Team> teamsInEncount = new List<Team>();
                Team first = teamsVector[i];
                sport.AddTeam(first);
                teamsInEncount.Add(first);

                for (int j = i; j < amountTeams; j++)
                {
                    if (i != j)
                    {
                        Team second = teamsVector[j];
                        sport.AddTeam(second);
                        teamsInEncount.Add(second);

                        encounter = new Encounter(sport, teamsInEncount, start);
                        encounters.Add(encounter);
                    }
                }
            }
        }

        private Team[] ConvertCollectionOfTeamToVector(IEnumerable<Team> teams)
        {
            int amountTeams = teams.ToList().Count;
            Team[] teamsVector = new Team[amountTeams];

            int i = 0;
            foreach (Team team in teams)
            {
                teamsVector[i] = team;
                i++;
            }

            return teamsVector;
        }
    }
}