using System;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("EirinDuran.ServicesTest")]

namespace EirinDuran.Services.DTO_Mappers
{
    internal class EncounterMapper : DTOMapper<Encounter, EncounterDTO>
    {
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private IRepository<Comment> commentRepo;
        private TeamMapper teamMapper;

        public EncounterMapper(IRepository<Sport> sportRepo, IRepository<Team> teamRepo, IRepository<Comment> commentRepo)
        {
            this.sportRepo = sportRepo;
            this.teamRepo = teamRepo;
            this.commentRepo = commentRepo;
            teamMapper = new TeamMapper(sportRepo);
        }

        public override EncounterDTO Map(Encounter encounter)
        {
            Dictionary<TeamDTO, int> results = new Dictionary<TeamDTO, int>();
            encounter.Results.ToList().ForEach(p => results.Add(teamMapper.Map(p.Key), p.Value));
            return new EncounterDTO()
            {
                Id = encounter.Id,
                DateTime = encounter.DateTime,
                CommentsIds = encounter.Comments.Select(comment => comment.Id).ToList(),
                SportName = encounter.Sport.Name,
                TeamIds = encounter.Teams.Select(t => t.Name + "_" + t.Sport.Name).ToList(),
                Results = results
            };
        }

        protected override Encounter TryToMapModel(EncounterDTO encounterDTO)
        {
            Dictionary<Team, int> results = new Dictionary<Team, int>();
            encounterDTO.Results.ToList().ForEach(p => results.Add(teamRepo.Get(p.Key.Name + "_" + p.Key.SportName), p.Value));
            
            Encounter encounter = new Encounter(
                id: encounterDTO.Id,
                teams: encounterDTO.TeamIds.Select(t => teamRepo.Get(t)),
                comments: encounterDTO.CommentsIds.ToList().ConvertAll(comment => commentRepo.Get(comment.ToString())),
                dateTime: encounterDTO.DateTime,
                sport: sportRepo.Get(encounterDTO.SportName),
                results: results
            );
            return encounter;
        }
    }
}
