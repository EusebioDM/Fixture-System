﻿using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess.Entities.Mappers
{
    internal class UserMapper
    {
        public UserEntity Map(User user)
        {
            UserEntity userEntity = new UserEntity()
            {
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Password = user.Password,
                Mail = user.Mail,
                Role = user.Role
            };
            List<TeamUserEntity> teamUsersRelations = user.FollowedTeams.ToList().ConvertAll(t => new TeamUserEntity(
                team: new TeamEntity(t),
                user: userEntity
            ));
            userEntity.TeamUsers = teamUsersRelations;
            return userEntity;
        }

        public User Map(UserEntity entity)
        {
            return new User(
                role: entity.Role,
                userName: entity.UserName,
                name: entity.Name,
                surname: entity.Surname,
                password: entity.Password,
                mail: entity.Mail,
                followedTeams: entity.TeamUsers.Select(eu => eu.Team.ToModel()).ToList()
            );
        }

        public void Update(User source, UserEntity destination)
        {
            destination.UserName = source.UserName;
            destination.Name = source.Name;
            destination.Surname = source.Surname;
            destination.Password = source.Password;
            destination.Mail = source.Mail;
            destination.Role = source.Role;
            destination.TeamUsers = source.FollowedTeams.Select(t => new TeamUserEntity()
            {
                Team = new TeamEntity(t),
                TeamName = t.Name,
                User = Map(source),
                UserNamee = source.UserName,
                SportName = t.Sport.Name
            }).ToList();
        }
    }
}