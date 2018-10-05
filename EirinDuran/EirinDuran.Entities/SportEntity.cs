using EirinDuran.Domain.Fixture;
using EirinDuran.Entities.Mappers;
using System.Collections.Generic;

namespace EirinDuran.Entities
{
    public class SportEntity : IEntity<Sport>
    {
        public string TeamName { get; set; } // Key cant have the same property name as Team because of EF for no good reason at all
        private SportMapper mapper;

        public SportEntity()
        {
            mapper = new SportMapper();
        }

        public SportEntity(Sport sport) : this()
        {
            UpdateWith(sport);
        }

        public void UpdateWith(Sport sport)
        {
            mapper.Update(sport,this);
        }

        public Sport ToModel()
        {
            return mapper.Map(this);
        }

        public override bool Equals(object obj)
        {
            var entity = obj as SportEntity;
            return entity != null &&
                   TeamName == entity.TeamName;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(TeamName);
        }
    }
}
