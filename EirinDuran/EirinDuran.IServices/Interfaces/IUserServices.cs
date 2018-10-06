using EirinDuran.IServices.DTOs;
using System.Collections.Generic;

namespace EirinDuran.IServices.Interfaces
{
    public interface IUserServices
    {
        void CreateUser(UserDTO userDTO);

        UserDTO GetUser(string userName);

        IEnumerable<UserDTO> GetAllUsers();

        void DeleteUser(string userName);

        void ModifyUser(UserDTO userDTO);

        IEnumerable<TeamDTO> GetFollowedTeams();
    }
}
