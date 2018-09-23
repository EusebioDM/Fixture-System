using EirinDuran.Domain.User;
using EirinDuran.IServices;
using EirinDuran.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Services.DTO_Mappers
{
    internal class UserMapper
    {
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
                FollowedTeams = user.FollowedTeams.Select(userFollowedTeam => new TeamDTO()
                {
                    Name = userFollowedTeam.Name,
                    Logo = userFollowedTeam.Logo
                }).ToList()
            };
        }

        public User Map(UserDTO userDTO)
        {
            return new User(userName: userDTO.UserName,
                name: userDTO.Name, 
                role: userDTO.IsAdmin ? Role.Administrator : Role.Follower,
                surname: userDTO.Surname, password: userDTO.Password, 
                mail: userDTO.Mail, followedTeams: userDTO.FollowedTeams
                .Select(userDTOFollowedTeam => new Domain.Fixture.Team(name: userDTOFollowedTeam.Name, logo: userDTOFollowedTeam.Logo))
                .ToList());
        }
    }
}
