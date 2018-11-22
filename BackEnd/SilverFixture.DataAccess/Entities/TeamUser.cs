using SilverFixture.Domain.Fixture;
using SilverFixture.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SilverFixture.DataAccess.Entities
{
    public class TeamUser
    {
        public string TeamName { get; set; }
        public string SportName { get; set; }
        public virtual TeamEntity Team { get; set; }
        public virtual UserEntity User { get; set; }
        public string UserName { get; set; }

        public TeamUser()
        {

        }

        public TeamUser(TeamEntity team, UserEntity user)
        {
            Team = team;
            User = user;
            TeamName = Team.Name;
            UserName = User.UserName;
        }
    }
}
