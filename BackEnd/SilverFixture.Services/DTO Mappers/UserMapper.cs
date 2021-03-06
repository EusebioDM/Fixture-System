﻿using SilverFixture.Domain.Fixture;
using SilverFixture.Domain.User;
using SilverFixture.IDataAccess;
using SilverFixture.IServices;
using SilverFixture.IServices.DTOs;
using SilverFixture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverFixture.Services.DTO_Mappers
{
    internal class UserMapper : DTOMapper<User, UserDTO>
    {
        private IRepository<Team> teamRepo;

        public UserMapper(IRepository<Team> teamRepo){
            this.teamRepo = teamRepo;
        }
        public override UserDTO Map(User user)
        {
            return new UserDTO()
            {
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Password = user.Password,
                Mail = user.Mail,
                IsAdmin = user.Role == Role.Administrator,
                FollowedTeamsNames = user.FollowedTeams.Select(userFollowedTeam => userFollowedTeam.Name.ToString()).ToList()
            };
        }

        protected override User TryToMapModel(UserDTO userDTO)
        {
            return new User(userName: userDTO.UserName,
                name: userDTO.Name, 
                role: userDTO.IsAdmin ? Role.Administrator : Role.Follower,
                surname: userDTO.Surname, password: userDTO.Password, 
                mail: userDTO.Mail, 
                followedTeams: userDTO.FollowedTeamsNames.ConvertAll(name => teamRepo.Get(name))
            );
        }
    }
}
