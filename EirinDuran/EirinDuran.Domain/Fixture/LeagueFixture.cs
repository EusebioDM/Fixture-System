using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class LeagueFixture : IFixtureGenerator
    {
        public string Description => throw new NotImplementedException();

        public ICollection<Encounter> GenerateFixture(IEnumerable<Team> teams, DateTime start)
        {
            List<Encounter> encounters = new List<Encounter>();
            List<Team> teamList = teams.ToList();
            int amountTeams = teamList.Count;

            if (amountTeams % 2 == 0)
            {
                Team[] teamsVector = new Team[amountTeams];

                int i = 0;
                foreach (Team team in teamList)
                {
                    teamsVector[i] = team;
                    i++;
                }

                Encounter encounter;
                Sport sport = new Sport("football");

                for (int j = 0; j < amountTeams; j++)
                {
                    List<Team> teamsInEncount = new List<Team>();
                    Team first = teamsVector[j];
                    sport.AddTeam(first);
                    teamsInEncount.Add(first);

                    for (int k = j; k < amountTeams; k++)
                    {
                        if (j != k)
                        {
                            Team second = teamsVector[k];
                            sport.AddTeam(second);
                            teamsInEncount.Add(second);

                            encounter = new Encounter(sport, teamsInEncount, new DateTime(2018, 10, 07, 18, 30, 00));
                            encounters.Add(encounter);
                        }
                    }
                }
            }
            else
            {
                throw new InvalidNumberOfTeamsException();
            }
            
            return encounters;
        }
    }
}