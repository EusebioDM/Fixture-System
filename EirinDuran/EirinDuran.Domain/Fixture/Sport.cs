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

        private void SetNameIfValid(string value)
        {
            bool valid = validator.ValidateNotNullOrEmptyString(value);
            if (!valid)
            {
                throw new EmptyFieldException();
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
    }
}
