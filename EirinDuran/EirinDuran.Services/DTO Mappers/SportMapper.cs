using EirinDuran.Domain.Fixture;
using EirinDuran.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Services.DTO_Mappers
{
    internal class SportMapper
    {
        public SportDTO Map(Sport sport)
        {
            return new SportDTO()
            {
                Name = sport.Name,
                Teams = sport.Teams.Select(sportTeam => new TeamDTO()
                {
                    Name = sportTeam.Name,
                    Logo = sportTeam.Logo
                }).ToList()
            };
        }

        public Sport Map(SportDTO sportDTO)
        {
            return new Sport(name: sportDTO.Name, 
                teams: sportDTO.Teams.Select(sportDTOTeam => new Team(name: sportDTOTeam.Name, logo: sportDTOTeam.Logo))
                .ToList());
        }
    }
}
