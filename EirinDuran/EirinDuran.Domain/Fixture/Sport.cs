using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class Sport
    {
        private string name;
        private HashSet<Team> teams;
        private StringValidator validator;
        public string Name { get => name; set => SetNameIfValid(value); }
        public IEnumerable<Team> Teams { get => teams; }

        public Sport(string name)
        {
            validator = new StringValidator();
            teams = new HashSet<Team>();
            Name = name;
        }

        public Sport(string name, IEnumerable<Team> teams) : this(name)
        {
            foreach (Team team in teams)
                AddTeam(team);
        }

        private void SetNameIfValid(string value)
        {
            bool valid = validator.ValidateNotNullOrEmptyString(value);
            if (!valid)
            {
                throw new EmptyFieldException("Name");
            }
            else
            {
                name = value;
            }
        }

        public void AddTeam(Team team)
        {
            teams.Add(team);
        }

        public void RemoveTeam(Team boca)
        {
            teams.Remove(boca);
        }

        public override bool Equals(object obj)
        {
            var sport = obj as Sport;
            return sport != null &&
                   Name == sport.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
