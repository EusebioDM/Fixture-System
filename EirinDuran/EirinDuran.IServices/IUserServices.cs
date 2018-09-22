using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using System.Collections.Generic;

public interface IUserServices
{
    void AddUser(User user);
    User GetUser(User user);
    IEnumerable<User> GetAllUsers();
    void DeleteUser(string userName);
    void Modify(User userToModify);
    void AddFollowedTeam(Team team);
    IEnumerable<Team> GetAllFollowedTeams();
}