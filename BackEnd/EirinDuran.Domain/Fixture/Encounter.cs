using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class Encounter
    {
        public Guid Id { get; private set; }
        public DateTime DateTime { get => dateTime; set => SetDateIfValid(value); }
        public IEnumerable<Team> Teams => teams;
        public IEnumerable<Comment> Comments => comments;
        public Dictionary<Team, int> Results => new Dictionary<Team, int>(results);
        public Sport Sport { get; private set; }
        private ICollection<Team> teams;
        private DateTime dateTime;
        private ICollection<Comment> comments;
        private Dictionary<Team, int> results;

        public Encounter(Sport sport, IEnumerable<Team> teams, DateTime dateTime)
        {
            comments = new List<Comment>();
            Sport = sport;
            ValidateNumberOfTeams(teams);
            this.teams = GetTeamsArray(teams);
            DateTime = dateTime;
            Id = Guid.NewGuid();
            results = new Dictionary<Team, int>();
        }

        public Encounter(Guid id, Sport sport, IEnumerable<Team> teams, DateTime dateTime, ICollection<Comment> comments, Dictionary<Team,int> results) : this(sport, teams,dateTime)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            this.comments = comments;
            this.results = results;
        }

        public void AddComment(User.User user, string message)
        {
            comments.Add(new Comment(user, message));
        }

        private void ValidateNumberOfTeams(IEnumerable<Team> teams)
        {
            if(Sport.EncounterPlayerCount == EncounterPlayerCount.TwoPlayers && teams.Count() != 2)
            {
                throw new InvalidNumberOfTeamsException();
            }
        }

        private List<Team> GetTeamsArray(IEnumerable<Team> teams)
        {
            List<Team> teamList = teams.ToList();
            teamList.ForEach(ValidateTeamIsValid);
            return teamList;
        }

        private void ValidateTeamIsValid(Team team)
        {
            if (!team.Sport.Equals(Sport))
                throw new InvalidTeamException();
        }

        private void SetDateIfValid(DateTime date)
        {
            if (date < DateTime.Now)
                throw new InvalidDateException();
            else
                dateTime = date;
        }

        public void AddOrReplaceResult(Team team, int position)
        {
            ValidateTeamIsInEncounter(team);
            results[team] = position;
        }

        private void ValidateTeamIsInEncounter(Team team)
        {
            if (!teams.Contains(team))
                throw new InvalidTeamException();
        }

        protected bool Equals(Encounter other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Encounter) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
