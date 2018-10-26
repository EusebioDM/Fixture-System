using EirinDuran.Domain.Fixture;
using EirinDuran.DataAccess.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.GenericEntityRepository;

namespace EirinDuran.DataAccess.Entities
{
    public class EncounterEntity : IEntity<Encounter>
    {

        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public virtual SportEntity Sport { get; set; }
        public virtual TeamEntity HomeTeam { get; set; }
        public virtual TeamEntity AwayTeam { get; set; }
        public virtual ICollection<CommentEntity> Comments { get; set; }
        private EncounterMapper mapper;

        public EncounterEntity()
        {
            mapper = new EncounterMapper();
            Comments = new List<CommentEntity>();
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
