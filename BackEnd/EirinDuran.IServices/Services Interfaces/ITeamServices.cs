using System.Collections.Generic;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Services_Interfaces
{
    public interface ITeamServices
    {
        TeamDTO CreateTeam(TeamDTO team);

        TeamDTO GetTeam(string teamId);

        IEnumerable<TeamDTO> GetAllTeams();

        void UpdateTeam(TeamDTO teamToUpdate);

        void DeleteTeam(string teamId);

        void AddFollowedTeam(string teamId);
        
        void DeleteFollowedTeam(string teamId);
    }
}
