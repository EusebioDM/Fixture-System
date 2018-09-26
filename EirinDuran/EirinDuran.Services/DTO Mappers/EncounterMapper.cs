using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("EirinDuran.ServicesTest")]

namespace EirinDuran.Services.DTO_Mappers
{
    internal class EncounterMapper
    {
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private IRepository<Comment> commentRepo;

        public EncounterMapper(IRepository<Sport> sportRepo, IRepository<Team> teamRepo)
        {
            this.sportRepo = sportRepo;
            this.teamRepo = teamRepo;
        }

        public EncounterDTO Map(Encounter encounter)
        {
            return new EncounterDTO()
            {
                Id = encounter.Id,
                DateTime = encounter.DateTime,
                CommentsIds = encounter.Comments.Select(comment => comment.Id).ToList(),
                SportName = encounter.Sport.Name,
                HomeTeamName = encounter.Teams.First().Name,
                AwayTeamName = encounter.Teams.Last().Name
            };
        }

        public Encounter Map(EncounterDTO encounterDTO)
        {
            return new Encounter(id: encounterDTO.Id,
                teams: new List<Team>() { teamRepo.Get(encounterDTO.HomeTeamName), teamRepo.Get(encounterDTO.AwayTeamName) },
                comments: encounterDTO.CommentsIds.ConvertAll(comment => commentRepo.Get(comment.ToString())),
                dateTime: encounterDTO.DateTime,
                sport: sportRepo.Get(encounterDTO.SportName)
            );
        }
    }
}
