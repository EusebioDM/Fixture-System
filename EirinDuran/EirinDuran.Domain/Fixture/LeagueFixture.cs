using System;
using System.Collections.Generic;
using System.Linq;

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

        public ICollection<Encounter> GenerateFixture(IEnumerable<Team> teams, DateTime start)
        {
            ControlValidParams(teams, start);

            List<Encounter> encounters = new List<Encounter>();
            List<Team> teamList = teams.ToList();

            GenerateLeagueFixture(encounters, teamList, start);

            return encounters;
        }

        private void ControlValidParams(IEnumerable<Team> teams, DateTime start)
        {
            int amountTeams = teams.ToList().Count;

            if((amountTeams == 0) || (amountTeams % 2 != 0))
            {
                throw new InvalidNumberOfTeamsException();
            }
        }

        private void GenerateLeagueFixture(List<Encounter> encounters, List<Team> teamList, DateTime start)
        {

            int amountTeams = teamList.Count;
            int necessaryEncounters = amountTeams * (amountTeams - 1) / 2;
            int maxEncountersPerDay = amountTeams / 2;
            int necessaryRounds = amountTeams - 1;

            Encounter enconter;

            Team[] local = ConvertCollectionOfTeamToVector(teamList, 0, (amountTeams / 2));
            Team[] visitant = ConvertCollectionOfTeamToVector(teamList, (amountTeams / 2), amountTeams);

            for (int i = 0; i < necessaryRounds; i++)
            {
                for (int j = 0; j < (amountTeams / 2); j++)
                {
                    IEnumerable<Team> teamsIn = new List<Team>() { local[j], visitant[j] };
                    enconter = new Encounter(sport, teamsIn, start);
                    encounters.Add(enconter);
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