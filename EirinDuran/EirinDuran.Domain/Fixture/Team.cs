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


    }
}
