using EirinDuran.Domain.Fixture;
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
        public override TeamDTO Map(Team team)
        {
            return new TeamDTO()
            {
                Name = team.Name,
                Logo = team.Logo
            };
        }

        protected override Team TryToMapModel(TeamDTO teamDTO)
        {
            return new Team(name: teamDTO.Name, logo: teamDTO.Logo);
        }
    }
}
