using EirinDuran.Domain.Fixture;
using EirinDuran.IServices.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices.Interfaces
{
    public interface ITeamServices
    {
        void AddTeam(Team team);

        TeamDTO GetTeam(string teamName);

        IEnumerable<TeamDTO> GetAll();

        void DeleteTeam(string teamName);
    }
}
