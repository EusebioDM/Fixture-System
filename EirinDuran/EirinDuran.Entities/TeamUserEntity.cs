using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

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

        public TeamUserEntity(TeamEntity team, UserEntity user)
        {
            Team = team;
            User = user;
            TeamName = Team.Name;
            UserName = User.UserName;
        }

        //public TeamUserEntity(Team team, User user)
        //{
        //    Team = new TeamEntity(team);
        //    User = new UserEntity(user);
        //    TeamName = Team.Name;
        //    UserName = User.UserName;
        //}
    }
}
