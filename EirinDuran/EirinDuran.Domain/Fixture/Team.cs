using System.Collections.Generic;
using System.Drawing;

namespace EirinDuran.Domain.Fixture
{
    public class Team
    {
        public string Name { get => name; set => SetNameIfValid(value); }
        private string name;
        public Sport Sport { get; set; }
        public Image Logo { get; set; }
        private StringValidator validator;

        public Team()
        {
            Logo = GetDefaultImage();
        }

        public Team(string name, Sport sport) : this()
        {
            validator = new StringValidator();
            Name = name;
            Sport = sport;
        }

        private Image GetDefaultImage()
        {
            Bitmap bitmap = new Bitmap(500, 500);
            return bitmap;
        }

        public Team(string name, Sport sport, Image logo) : this(name, sport)
        {
            Logo = logo;
        }

        private void SetNameIfValid(string value)
        {
            bool valid = validator.ValidateNotNullOrEmptyString(value);
            if (!valid)
                throw new EmptyFieldException("Name");
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
