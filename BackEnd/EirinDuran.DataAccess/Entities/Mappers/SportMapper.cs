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
                SportName = sport.Name
            };
        }

        public Sport Map(SportEntity entity)
        {
            return new Sport( name: entity.SportName);
        }

        public void Update(Sport source, SportEntity destination)
        {
            destination.SportName = source.Name;
        }
    }
}
