using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Services.DTO_Mappers
{
    internal class TeamMapper : DTOMapper<Team, TeamDTO>
    {
        private IRepository<Sport> repo;

        public TeamMapper(IRepository<Sport> sportRepo){
            repo = sportRepo;
        }
        public override TeamDTO Map(Team team)
        {
            return new TeamDTO()
            {
                Name = team.Name,
                Logo = team.Logo,
                SportName = team.Sport.Name
            };
        }

        protected override Team TryToMapModel(TeamDTO teamDTO)
        {
            Sport sport = repo.Get(teamDTO.SportName);
            return new Team(name: teamDTO.Name,sport: sport,logo: teamDTO.Logo);
        }
    }
}
