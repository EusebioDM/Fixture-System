using EirinDuran.Domain.Fixture;
using EirinDuran.DataAccess.Entities.Mappers;
using System.Collections.Generic;
using EirinDuran.GenericEntityRepository;

namespace EirinDuran.DataAccess.Entities
{
    public class TeamEntity : IEntity<Team>
    {
        public string Name { get; set; }
        public string SportName { get; set; }
        public byte[] Logo { get; set; }
        public virtual SportEntity Sport { get; set; }
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
            return obj is TeamEntity other &&
                   Name == other.Name &&
                   SportName == other.SportName;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }


    }
}
