using EirinDuran.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices.Interfaces
{
    public interface ITeamServices
    {
        void AddTeam(Team team);

        Team GetTeam(string teamName);

        IEnumerable<Team> GetAll();

        void DeleteTeam(string teamName);
    }
}
