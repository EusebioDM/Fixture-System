using EirinDuran.IServices.DTOs;
using System.Collections.Generic;

namespace EirinDuran.IServices.Interfaces
{
    public interface ITeamServices
    {
        TeamDTO CreateTeam(TeamDTO team);

        TeamDTO GetTeam(string teamId);

        IEnumerable<TeamDTO> GetAllTeams();

        void UpdateTeam(TeamDTO teamToUpdate);

        void DeleteTeam(string teamId);

        void AddFollowedTeam(string sportId_teamName);
    }
}
