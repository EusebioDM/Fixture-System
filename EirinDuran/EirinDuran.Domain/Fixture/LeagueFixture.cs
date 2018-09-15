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
            else if (start > end)
            {
                throw new OutdatedDatesException();
            }
        }

        private void GenerateLeagueFixture(List<Encounter> encounters, List<Team> teamList, DateTime start, DateTime end)
        {

            int amountTeams = teamList.Count;
            Encounter enconter;

            int necessaryEncounters = amountTeams * (amountTeams - 1) / 2;
            int maxEncountersPerDay = amountTeams / 2;
            int necessaryRounds = necessaryEncounters / maxEncountersPerDay;

            Team[] local = ConvertCollectionOfTeamToVector(teamList, 0, amountTeams / 2);
            Team[] visitant = ConvertCollectionOfTeamToVector(teamList, amountTeams / 2, amountTeams);

            for (int i = 0; i < necessaryRounds; i++)
            {
                for (int j = 0; j < amountTeams / 2; j++)
                {
                    IEnumerable<Team> teamsIn = new List<Team>() { local[j], visitant[j] };
                    enconter = new Encounter(sport, teamsIn, start);
                    encounters.Add(enconter);
                }

                //incrementar el día
                start = start.AddDays(1);

                //for (int k = 0; k < (amountTeams / 2) - 1; k++)
                //{
                    //me guardo la ultima posicion del local
                    Team lastOfLocal = local[(amountTeams / 2) - 1];

                    //correr un lugar al local desde la posición 1 a la derecha 
                    for (int l = 1; l < (amountTeams / 2) - 1; l++)
                    {
                        local[l + 1] = local[l];
                    }

                    //coloco el 0 del visitante en la posición 1 del local
                    if ((amountTeams / 2) - 1 > 0)
                    {
                        local[1] = visitant[0];
                    }

                    //corro un lugar a la izquierda el visitante 
                    for (int g = 0; g < (amountTeams / 2) - 1; g++)
                    {
                        visitant[g] = visitant[g + 1]; 
                    }

                    //pongo en la ultima posición del visitante el local que me había guardado
                    if ((amountTeams / 2) - 1 > 0)
                    {
                        visitant[(amountTeams / 2) - 1] = lastOfLocal;
                    }
                //}
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