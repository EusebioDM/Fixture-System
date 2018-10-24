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
            return new EncounterEntity()
            {
                DateTime = encounter.DateTime,
                Sport = sportMapper.Map(encounter.Sport),
                HomeTeam = new TeamEntity(encounter.Teams.First()),
                AwayTeam = new TeamEntity(encounter.Teams.Last()),
                Id = encounter.Id,
                Comments = encounter.Comments.Select(c => new CommentEntity(c)).ToList()
            };
        }

        public Encounter Map(EncounterEntity entity)
        {
            IEnumerable<Team> teams = new List<Team>() { entity.HomeTeam.ToModel(), entity.AwayTeam.ToModel() };
            ICollection<Comment> comments = entity.Comments.Select(t => t.ToModel()).ToList();
            Sport sport = sportMapper.Map(entity.Sport);

            return new Encounter(entity.Id, sport, teams, entity.DateTime, comments);
        }

        public void Update(Encounter source, EncounterEntity destination)
        {
            List<TeamEntity> teams = source.Teams.Select(sourcemTeam => new TeamEntity(sourcemTeam)).ToList(); 
            destination.DateTime = source.DateTime;
            destination.Sport = new SportEntity(source.Sport);
            destination.HomeTeam = teams.First();
            destination.AwayTeam = teams.Last();
            destination.Id = source.Id;
            destination.Comments = source.Comments.Select(c => new CommentEntity(c)).ToList();
        }
    }
}
