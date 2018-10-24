using EirinDuran.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess.Entities.Mappers
{
    internal class SportMapper
    {
        private TeamMapper teamMapper = new TeamMapper();

        public SportEntity Map(Sport sport)
        {
            return new SportEntity()
            {
                SportName = sport.Name,
                EncounterPlayerCount = sport.EncounterPlayerCount
            };
        }

        public Sport Map(SportEntity entity)
        {
            return new Sport( name: entity.SportName, encounterPlayerCount: entity.EncounterPlayerCount);
        }

        public void Update(Sport source, SportEntity destination)
        {
            destination.SportName = source.Name;
            destination.EncounterPlayerCount = source.EncounterPlayerCount;
        }
    }
}
