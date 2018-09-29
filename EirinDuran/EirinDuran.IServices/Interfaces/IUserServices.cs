using EirinDuran.IServices.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices.Interfaces
{
    public interface IUserServices
    {
        void CreateUser(UserDTO userDTO);

        UserDTO GetUser(string username);

        IEnumerable<UserDTO> GetAllUsers();

        void DeleteUser(string userName);

        void Modify(UserDTO userDTO);

        void AddFollowedTeam(string id);

        IEnumerable<TeamDTO> GetAllFollowedTeams();

    }
}
