using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EirinDuran.DataAccess.Entities
{
    public class TeamUserEntity
    {
        public string TeamName { get; set; }
        public string SportName { get; set; }
        public virtual TeamEntity Team { get; set; }
        public virtual UserEntity User { get; set; }
        public string UserNamee { get; set; }

        public TeamUserEntity()
        {

        }

        public TeamUserEntity(TeamEntity team, UserEntity user)
        {
            Team = team;
            User = user;
            TeamName = Team.Name;
            UserNamee = User.UserName;
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
