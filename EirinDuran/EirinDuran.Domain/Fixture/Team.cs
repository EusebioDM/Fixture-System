using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class Team
    {
        public string Name { get; set; }
        public Image Logo { get; set; }

        public Team(string name, Image logo)
        {
            Name = name;
            Logo = logo;
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
