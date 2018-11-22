using SilverFixture.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Text;
using SilverFixture.GenericEntityRepository;
using SilverFixture.DataAccess.Entities.Mappers;

namespace SilverFixture.DataAccess.Entities
{
    public class EncounterEntity : IEntity<Encounter>
    {

        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public virtual SportEntity Sport { get; set; }
        public virtual ICollection<EncounterTeam> Teams { get; set; }
        public virtual ICollection<CommentEntity> Comments { get; set; }
        public virtual ICollection<TeamResult> Results { get; set; }
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
            return obj is EncounterEntity entity &&
                   Id.Equals(entity.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        }
    }
}
