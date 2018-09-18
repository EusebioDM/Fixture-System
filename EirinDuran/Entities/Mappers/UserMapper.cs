﻿using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace EirinDuran.Entities.Mappers
{
    internal class UserMapper
    {
        public UserEntity Map(User user)
        {
            return new UserEntity()
            {
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Password = user.Password,
                Mail = user.Mail,
                Role = user.Role,
                Id = user.Id,
                FollowedTeams = user.FollowedTeams.Select(t => new TeamEntity(t)).ToList()
            };
        }

        public User Map(UserEntity entity)
        {
            ICollection<Team> teams = entity.FollowedTeams.Select(t => t.ToModel()).ToList();
            return new User(id: entity.Id, role: entity.Role, userName: entity.UserName, name: entity.Name, surname: entity.Surname, password: entity.Password, mail: entity.Mail, followedTeams: teams);
        }

        public void Update(User source, UserEntity destination)
        {
            destination.UserName = source.UserName;
            destination.Name = source.Name;
            destination.Surname = source.Surname;
            destination.Password = source.Password;
            destination.Mail = source.Mail;
            destination.Role = source.Role;
            destination.Id = source.Id;
            destination.FollowedTeams = source.FollowedTeams.Select(t => new TeamEntity(t)).ToList();
        }
    }
}
