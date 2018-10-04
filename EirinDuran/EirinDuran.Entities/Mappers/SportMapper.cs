using EirinDuran.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Entities.Mappers
{
    internal class SportMapper
    {
        private TeamMapper teamMapper = new TeamMapper();

        public SportEntity Map(Sport sport)
        {
            return new SportEntity()
            {
                Name = sport.Name
            };
        }

        public Sport Map(SportEntity entity)
        {
            return new Sport( name: entity.Name);
        }

        public void Update(Sport source, SportEntity destination)
        {
            destination.Name = source.Name;
        }
    }
}
