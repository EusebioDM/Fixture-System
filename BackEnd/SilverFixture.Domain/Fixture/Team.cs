using System.Collections.Generic;
using System.Drawing;

namespace SilverFixture.Domain.Fixture
{
    public class Team
    {
        public Name Name { get; set; }
        public Sport Sport { get; set; }
        public Image Logo { get; set; }

        public Team()
        {
            Logo = GetDefaultImage();
        }

        public Team(string name, Sport sport) : this()
        {
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

        public override bool Equals(object obj)
        {
            return obj is Team other &&
                   Name == other.Name &&
                   Sport.Equals(other.Sport);
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
