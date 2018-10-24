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

        public EncounterMapper(IRepository<Sport> sportRepo, IRepository<Team> teamRepo)
        {
            this.sportRepo = sportRepo;
            this.teamRepo = teamRepo;
        }

        public override EncounterDTO Map(Encounter encounter)
        {
            return new EncounterDTO()
            {
                Id = encounter.Id,
                DateTime = encounter.DateTime,
                CommentsIds = encounter.Comments.Select(comment => comment.Id).ToList(),
                SportName = encounter.Sport.Name,
                TeamIds = encounter.Teams.Select(t => t.Name + "_" + t.Sport.Name).ToList()
            };
        }

        protected override Encounter TryToMapModel(EncounterDTO encounterDTO)
        {
            return new Encounter(id: encounterDTO.Id,
                teams: encounterDTO.TeamIds.Select(t => teamRepo.Get(t)),
                comments: encounterDTO.CommentsIds.ConvertAll(comment => commentRepo.Get(comment.ToString())),
                dateTime: encounterDTO.DateTime,
                sport: sportRepo.Get(encounterDTO.SportName)
            );
        }
    }
}
