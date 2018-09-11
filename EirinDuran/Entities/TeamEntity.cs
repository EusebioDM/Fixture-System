using EirinDuran.Domain.Fixture;
using EirinDuran.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Entities
{
    public class TeamEntity
    {
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        private TeamMapper mapper;

        public TeamEntity()
        {
            mapper = new TeamMapper();
        }

        public TeamEntity(Team team) : this()
        {
            UpdateWith(team);
        }

        public void UpdateWith(Team team)
        {
            TeamMapper mapper = new TeamMapper();
            mapper.Update(team, this);
        }

        public Team ToModel()
        {
            return mapper.Map(this);
        }

        public override bool Equals(object obj)
        {
            var entity = obj as TeamEntity;
            return entity != null &&
                   Name == entity.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
