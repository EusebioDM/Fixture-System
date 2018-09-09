using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class Encounter
    {
        private Team[] teams;
        private DateTime dateTime;
        public DateTime DateTime { get => dateTime; set => SetDateIfValid(value); }

        public Sport Sport { get; set; }

        public Encounter(Sport sport, IEnumerable<Team> teams, DateTime dateTime)
        {
            ValidateNumberOfTeams(teams);
            Sport = sport;
            this.teams = GetTeamsArray(teams);
            DateTime = dateTime;
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

        private void SetDateIfValid(DateTime date)
        {
            if (date < DateTime.Now)
                throw new InvalidDateException();
            else
                dateTime = date;
        }
    }
}
