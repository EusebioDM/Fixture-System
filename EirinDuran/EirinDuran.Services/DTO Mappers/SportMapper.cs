using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Services.DTO_Mappers
{
    internal class SportMapper
    {
        private IRepository<Team> teamRepo;

        public SportMapper(IRepository<Team> teamRepo){
            this.teamRepo = teamRepo;
        }
        public SportDTO Map(Sport sport)
        {
            return new SportDTO()
            {
                Name = sport.Name,
                TeamsNames = sport.Teams.Select(team => team.Name).ToList()
            };
        }

        public Sport Map(SportDTO sportDTO)
        {
            return new Sport(name: sportDTO.Name, 
                teams: sportDTO.TeamsNames.ConvertAll(name => teamRepo.Get(name))
            );
        }
    }
}
