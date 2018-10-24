using EirinDuran.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess.Entities.Mappers
{
    internal class EncounterMapper
    {
        private readonly SportMapper sportMapper = new SportMapper();
        private readonly TeamMapper teamMapper = new TeamMapper();

        public EncounterEntity Map(Encounter encounter)
        {
            IEnumerable<TeamEntity> teams = encounter.Teams.Select(t => teamMapper.Map(t));
            EncounterEntity encounterEntity = new EncounterEntity()
            {
                DateTime = encounter.DateTime,
                Sport = sportMapper.Map(encounter.Sport),
                Id = encounter.Id,
                Comments = encounter.Comments.Select(c => new CommentEntity(c)).ToList()
            };
            encounterEntity.Teams = encounter.Teams.Select(t => new EncounterTeam(encounterEntity, new TeamEntity(t))).ToList();

            return encounterEntity;
        }

        public Encounter Map(EncounterEntity entity)
        {
            IEnumerable<Team> teams = entity.Teams.Select(t => t.Team.ToModel());
            ICollection<Comment> comments = entity.Comments.Select(t => t.ToModel()).ToList();
            Sport sport = sportMapper.Map(entity.Sport);

            return new Encounter(entity.Id, sport, teams, entity.DateTime, comments);
        }

        public void Update(Encounter source, EncounterEntity destination)
        {
            List<TeamEntity> teams = source.Teams.Select(sourcemTeam => new TeamEntity(sourcemTeam)).ToList(); 
            destination.DateTime = source.DateTime;
            destination.Sport = new SportEntity(source.Sport);
            destination.Id = source.Id;
            destination.Comments = source.Comments.Select(c => new CommentEntity(c)).ToList();
            destination.Teams = source.Teams.Select(t => new EncounterTeam(destination, new TeamEntity(t))).ToList();
        }
    }
}
