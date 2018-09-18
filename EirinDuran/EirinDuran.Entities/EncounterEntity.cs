using EirinDuran.Domain.Fixture;
using EirinDuran.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Entities
{
    public class EncounterEntity : IEntity<Encounter>
    {
        
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public virtual SportEntity Sport { get; set; }
        public virtual ICollection<TeamEntity> Teams { get; set; }
        private EncounterMapper mapper;

        public EncounterEntity()
        {
            mapper = new EncounterMapper();
            Teams = new List<TeamEntity>();
        }

        public EncounterEntity(Encounter encounter) : this()
        {
            UpdateWith(encounter);
        }

        public void UpdateWith(Encounter encounter)
        {
            mapper.Update(encounter, this);
        }

        public Encounter ToModel()
        {
            return mapper.Map(this);
        }
        public string GetAlternateKey() => Id.ToString();

        public string NavegablePropeties => "Teams";

        public override bool Equals(object obj)
        {
            var entity = obj as EncounterEntity;
            return entity != null &&
                   Id.Equals(entity.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        }
    }
}
