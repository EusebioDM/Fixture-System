using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class Encounter
    {
        public Guid Id { get; private set; }
        private Team[] teams;
        private DateTime dateTime;
        public DateTime DateTime { get => dateTime; set => SetDateIfValid(value); }
        public IEnumerable<Team> Teams => teams;

        public Sport Sport { get; set; }

        public Encounter(Sport sport, IEnumerable<Team> teams, DateTime dateTime)
        {
            Id = Guid.NewGuid();
            ValidateNumberOfTeams(teams);
            Sport = sport;
            this.teams = GetTeamsArray(teams);
            DateTime = dateTime;
            Id = Guid.Empty;
        }

        public Encounter(Guid id, Sport sport, IEnumerable<Team> teams, DateTime dateTime)
        {
            Id = id;
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
                ValidateTeamIsValid(team);
                array[i++] = team;
            }
            return array;
        }

        private void ValidateTeamIsValid(Team team)
        {
            if (!Sport.Teams.Contains(team))
                throw new InvalidTeamException();
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
