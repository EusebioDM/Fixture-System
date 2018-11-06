using System.Collections.Generic;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Services_Interfaces
{
    public interface IUserServices
    {
        UserDTO CreateUser(UserDTO userDTO);

        UserDTO GetUser(string userName);

        IEnumerable<UserDTO> GetAllUsers();

        void DeleteUser(string userName);

        void ModifyUser(UserDTO userDTO);

        IEnumerable<TeamDTO> GetFollowedTeams();
    }
}
