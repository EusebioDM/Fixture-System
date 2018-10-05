using EirinDuran.IServices.DTOs;
using System.Collections.Generic;

namespace EirinDuran.IServices.Interfaces
{
    public interface ITeamServices
    {
        void CreateTeam(TeamDTO team);

        TeamDTO GetTeam(string teamId);

        IEnumerable<TeamDTO> GetAllTeams();

        void DeleteTeam(string teamId);
    }
}
