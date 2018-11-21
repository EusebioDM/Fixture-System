using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverFixture.Domain.Fixture
{
    public class Encounter
    {
        public Guid Id { get; private set; }

        public DateTime DateTime
        {
            get => dateTime;
            set => SetDateIfValid(value);
        }

        public IEnumerable<Team> Teams => teams;
        public IEnumerable<Comment> Comments => comments;
        public Dictionary<Team, int> Results => new Dictionary<Team, int>(results);
        public Sport Sport { get; private set; }
        private DateTime dateTime;
        private readonly ICollection<Team> teams;
        private readonly ICollection<Comment> comments;
        private readonly Dictionary<Team, int> results;

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

        public Encounter(Guid id, Sport sport, IEnumerable<Team> teams, DateTime dateTime, ICollection<Comment> comments, Dictionary<Team, int> results) : this(sport, teams, dateTime)
        {
            ValidateResults(results);
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            this.comments = comments;
            this.results = results;
        }

        private void ValidateResults(Dictionary<Team, int> dictionary)
        {
            if (Sport.EncounterPlayerCount == EncounterPlayerCount.TwoPlayers)
            {
                ValidateTwoPlayerEncounter(dictionary);
            }
            else
            {
                ValidateMoreThanTwoPlayerEncounter(dictionary);
            }
        }

        private void ValidateTwoPlayerEncounter(Dictionary<Team, int> dictionary)
        {
            if (dictionary.Count != 0 && dictionary.Count != 2)
                throw new DomainException(dictionary, "there isnt a result for every team");
            if (dictionary.Keys.Any(team => !Teams.Contains(team)))
                throw new DomainException(dictionary, "a team in results not part of the encounter");
            if (dictionary.Values.Any(i => i < 1 || i > 2))
                throw new DomainException(dictionary, "result is not first or second");
        }

        private void ValidateMoreThanTwoPlayerEncounter(Dictionary<Team, int> dictionary)
        {
            if (dictionary.Count != 0 && dictionary.Count != teams.Count)
                throw new DomainException(dictionary, "there isnt a result for every team");
            if (dictionary.Keys.Any(team => !Teams.Contains(team)))
                throw new DomainException(dictionary, "a team in results not part of the encounter");
            if (dictionary.Values.Any(result => dictionary.Values.Count(r => r == result) != 1))
                throw new DomainException(dictionary, "repeated results");
            
        }

        public void AddComment(User.User user, string message)
        {
            comments.Add(new Comment(user, message));
        }

        private void ValidateNumberOfTeams(IEnumerable<Team> teams)
        {
            if (Sport.EncounterPlayerCount == EncounterPlayerCount.TwoPlayers && teams.Count() != 2)
            {
                throw new DomainException(teams.ToString(), "its a two player encounter and Teams didnt have 2 teams.");
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