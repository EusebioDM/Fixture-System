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
                Teams = teams
            };
        }

        public Encounter Map(EncounterEntity entity)
        {
            IEnumerable<Team> teams = entity.Teams.Select(t => teamMapper.Map(t));
            Sport sport = sportMapper.Map(entity.Sport);

            return new Encounter(sport, teams, entity.DateTime);
        }

        public void Update(Encounter sourcem, EncounterEntity destination)
        {
            destination.DateTime = sourcem.DateTime;
            destination.Sport = new SportEntity(sourcem.Sport);
            destination.Teams = sourcem.Teams.Select(sourcemTeam => new TeamEntity(sourcemTeam));
        }
    }
}
