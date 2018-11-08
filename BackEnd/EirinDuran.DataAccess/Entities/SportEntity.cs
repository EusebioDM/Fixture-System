using EirinDuran.Domain.Fixture;
using EirinDuran.DataAccess.Entities.Mappers;
using System.Collections.Generic;
using EirinDuran.GenericEntityRepository;

namespace EirinDuran.DataAccess.Entities
{
    public class SportEntity : IEntity<Sport>
    {
        public string SportName { get; set; }
        public EncounterPlayerCount EncounterPlayerCount { get; set; }
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
                   SportName == entity.SportName;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(SportName);
        }
    }
}
