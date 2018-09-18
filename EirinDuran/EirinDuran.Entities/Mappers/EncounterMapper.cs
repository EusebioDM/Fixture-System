using EirinDuran.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Entities.Mappers
{
    internal class EncounterMapper
    {
        private readonly SportMapper sportMapper = new SportMapper();
        private readonly TeamMapper teamMapper = new TeamMapper();

        public EncounterEntity Map(Encounter encounter)
        {
            IEnumerable<TeamEntity> teams = encounter.Teams.Select(t => teamMapper.Map(t));
            return new EncounterEntity()
            {
                DateTime = encounter.DateTime,
                Sport = sportMapper.Map(encounter.Sport),
                Teams = teams.ToList(),
                Id = encounter.Id,
                Comments = encounter.Comments.Select(c => new CommentEntity(c)).ToList()
            };
        }

        public Encounter Map(EncounterEntity entity)
        {
            IEnumerable<Team> teams = entity.Teams.Select(t => teamMapper.Map(t));
            ICollection<Comment> comments = entity.Comments.Select(t => t.ToModel()).ToList();
            Sport sport = sportMapper.Map(entity.Sport);

            return new Encounter(entity.Id, sport, teams, entity.DateTime, comments);
        }

        public void Update(Encounter source, EncounterEntity destination)
        {
            destination.DateTime = source.DateTime;
            destination.Sport = new SportEntity(source.Sport);
            destination.Teams = source.Teams.Select(sourcemTeam => new TeamEntity(sourcemTeam)).ToList();
            destination.Id = source.Id;
            destination.Comments = source.Comments.Select(c => new CommentEntity(c)).ToList();
        }
    }
}
