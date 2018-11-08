using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices.DTOs
{
    public class EncounterDTO
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<string> TeamIds { get; set; } = new List<string>();
        public ICollection<Guid> CommentsIds { get; set; } = new List<Guid>();
        public Dictionary<TeamDTO, int> Results { get; set; } = new Dictionary<TeamDTO, int>();
        public string SportName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is EncounterDTO dTO &&
                   Id.Equals(dTO.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        }
    }
}
