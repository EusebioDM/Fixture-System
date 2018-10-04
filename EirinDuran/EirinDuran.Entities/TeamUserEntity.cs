using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;

namespace EirinDuran.Entities
{
    public class TeamUserEntity
    {
        public string TeamName { get; set; }
        public virtual TeamEntity Team { get; set; }
        public virtual UserEntity User { get; set; }
        public string UserName { get; set; }

        public TeamUserEntity()
        {

        }

        public TeamUserEntity(Team team, User user)
        {
            Team = new TeamEntity(team);
            User = new UserEntity(user);
            TeamName = Team.Name;
            UserName = User.UserName;
        }
    }
}
