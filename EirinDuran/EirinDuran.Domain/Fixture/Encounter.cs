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
        public IEnumerable<Comment> Comments => comments;
        private ICollection<Comment> comments;

        public Sport Sport { get; set; }

        public Encounter(Sport sport, IEnumerable<Team> teams, DateTime dateTime)
        {
            comments = new List<Comment>();
            ValidateNumberOfTeams(teams);
            Sport = sport;
            this.teams = GetTeamsArray(teams);
            DateTime = dateTime;
            Id = Guid.NewGuid();
        }

        public Encounter(Guid id, Sport sport, IEnumerable<Team> teams, DateTime dateTime, ICollection<Comment> comments)
        {
            Id = id;
            ValidateNumberOfTeams(teams);
            Sport = sport;
            this.teams = GetTeamsArray(teams);
            this.comments = comments;
            DateTime = dateTime;
        }

        public void AddComment(User.User user, string message)
        {
            comments.Add(new Comment(user, message));
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
