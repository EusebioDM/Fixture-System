using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class Team
    {
        public string Name { get => name; set => SetNameIfValid(value); }
        private string name;
        public Image Logo { get; set; }
        private StringValidator validator;

        public Team(string name, Image logo)
        {
            validator = new StringValidator();
            Name = name;
            Logo = logo;
        }

        private void SetNameIfValid(string value)
        {
            bool valid = validator.ValidateNotNullOrEmptyString(value);
            if (!valid)
                throw new EmptyFieldException();
            else
                name = value;
        }

        public override bool Equals(object obj)
        {
            var team = obj as Team;
            return team != null &&
                   Name == team.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
