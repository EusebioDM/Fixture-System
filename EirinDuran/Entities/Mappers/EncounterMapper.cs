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
                Id = encounter.Id
            };
        }

        public Encounter Map(EncounterEntity entity)
        {
            IEnumerable<Team> teams = entity.Teams.Select(t => teamMapper.Map(t));
            Sport sport = sportMapper.Map(entity.Sport);

            return new Encounter(entity.Id, sport, teams, entity.DateTime);
        }

        public void Update(Encounter source, EncounterEntity destination)
        {
            destination.DateTime = source.DateTime;
            destination.Sport = new SportEntity(source.Sport);
            destination.Teams = source.Teams.Select(sourcemTeam => new TeamEntity(sourcemTeam)).ToList();
            destination.Id = source.Id;
        }
    }
}
