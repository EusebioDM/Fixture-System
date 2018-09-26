using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Services.DTO_Mappers
{
    internal class UserMapper
    {
        private IRepository<Team> teamRepo;

        public UserMapper(IRepository<Team> teamRepo){
            this.teamRepo = teamRepo;
        }
        public UserDTO Map(User user)
        {
            return new UserDTO()
            {
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Password = user.Password,
                Mail = user.Mail,
                IsAdmin = user.Role == Role.Administrator,
                FollowedTeamsNames = user.FollowedTeams.Select(userFollowedTeam => userFollowedTeam.Name).ToList()
            };
        }

        public User Map(UserDTO userDTO)
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
