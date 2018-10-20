using System;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.Domain.Fixture
{
    public class RoundRobinFixture : IFixtureGenerator
    {
        private Sport sport;

        public RoundRobinFixture(Sport sport)
        {
            this.sport = sport;
        }

        public ICollection<Encounter> GenerateFixture(IEnumerable<Team> teams, DateTime start)
        {
            List<Encounter> encounters = new List<Encounter>();
            List<Team> teamList = teams.ToList();

            bool areRepeatedTeams = teamList.GroupBy(n => n).Any(t => t.Count() > 1);

            if (areRepeatedTeams)
            {
                throw new ThereAreRepeatedTeamsException();
            }

            GenerateRoundRobinFixture(encounters, teamList, start);
         
            return encounters;
        }

        private void GenerateRoundRobinFixture(List<Encounter> encounters, List<Team> teamList, DateTime start)
        {

            int amountTeams = teamList.Count;

            int maxEncountersPerDay;
            int necessaryRounds;

            Team[] local;
            Team[] visitant;

            if (amountTeams % 2 == 0)
            {
                maxEncountersPerDay = amountTeams / 2;
                necessaryRounds = amountTeams - 1;

                local = ConvertCollectionOfTeamToVector(teamList, 0, (amountTeams / 2));
                visitant = ConvertCollectionOfTeamToVector(teamList, (amountTeams / 2), amountTeams);
            }
            else
            {
                maxEncountersPerDay = (amountTeams - 1) / 2;
                necessaryRounds = amountTeams;

                local = new Team[(amountTeams - 1) / 2 + 1];
                
                for (int i = 0; i < (amountTeams - 1) / 2; i++)
                {
                    local[i] = teamList[i];
                }

                visitant = ConvertCollectionOfTeamToVector(teamList, (amountTeams - 1) / 2, amountTeams);
            }

            Encounter encounter;
            
            for (int i = 0; i < necessaryRounds; i++)
            {
                for (int j = 0; j < maxEncountersPerDay; j++)
                {
                    if (local[j] != null && visitant[j] != null)
                    {
                        IEnumerable<Team> teamsIn = new List<Team>() { local[j], visitant[j] };
                        encounter = new Encounter(sport, teamsIn, start);
                        encounters.Add(encounter);
                    }
                }

                start = start.AddDays(1);

                Team lastOfLocal = local[(amountTeams / 2) - 1];
                
                for (int l = ((amountTeams / 2) - 1); l > 1; l--)
                {
                    local[l] = local[l - 1];
                }

                if ((amountTeams / 2) - 1 > 0)
                {
                    local[1] = visitant[0];
                }

                for (int g = 0; g < (amountTeams / 2) - 1; g++)
                {
                    visitant[g] = visitant[g + 1];
                }

                if ((amountTeams / 2) - 1 > 0)
                {
                    visitant[(amountTeams / 2) - 1] = lastOfLocal;
                }
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