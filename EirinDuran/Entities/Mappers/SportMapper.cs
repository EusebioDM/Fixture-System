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
                Name = sport.Name,
                Teams = sport.Teams.Select(sportTeam => teamMapper.Map(sportTeam)).ToList()
            };
        }

        public Sport Map(SportEntity entity)
        {
            IEnumerable<Team> teams = entity.Teams.Select(t => teamMapper.Map(t));
            return new Sport(name: entity.Name, teams: teams);
        }

        public void Update(Sport source, SportEntity destination)
        {
            destination.Name = source.Name;
            destination.Teams = source.Teams.Select(sourceTeam => new TeamEntity(sourceTeam)).ToList();
        }
    }
}
