using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class Encounter
    {
        private Sport sport;
        private Team[] teams;
        private DateTime dateTime;

        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        public Sport Sport { get => sport; set => sport = value; }

        public Encounter(Sport sport, IEnumerable<Team> teams, DateTime dateTime)
        {
            ValidateNumberOfTeams(teams);
            this.sport = sport;
            this.teams = GetTeamsArray(teams);
            this.dateTime = dateTime;
        }

        private void ValidateNumberOfTeams(IEnumerable<Team> teams)
        {
            if(teams.Count() != 2)
            {
                throw new InvalidNumberOfTeamsException();
            }
        }

        private Team[] GetTeamsArray(IEnumerable<Team> teams)
        {
            Team[] array = new Team[2];
            int i = 0;
            foreach (Team team in teams)
            {
                array[i++] = team;
            }
            return array;
        }
    }
}
