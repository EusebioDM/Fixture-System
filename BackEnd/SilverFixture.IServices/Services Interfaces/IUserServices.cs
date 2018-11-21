using System.Collections.Generic;
using SilverFixture.IServices.DTOs;

namespace SilverFixture.IServices.Services_Interfaces
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
