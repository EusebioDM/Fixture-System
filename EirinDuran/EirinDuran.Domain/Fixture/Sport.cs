using System.Collections.Generic;

namespace EirinDuran.Domain.Fixture
{
    public class Sport
    {
        private string name;
        private HashSet<Team> teams;
        private StringValidator validator;
        public string Name { get => name; set => SetNameIfValid(value); }

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
                throw new EmptyFieldException("Name");
            }
            else
            {
                name = value;
            }
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
