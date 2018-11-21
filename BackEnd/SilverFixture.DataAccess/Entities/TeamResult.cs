using System;

namespace SilverFixture.DataAccess.Entities
{
    public class TeamResult
    {
        public virtual TeamEntity Team { get; set; }
        public Guid EncounterId { get; set; }
        public string TeamId { get; set; }
        public int Position { get; set; }
    }
}