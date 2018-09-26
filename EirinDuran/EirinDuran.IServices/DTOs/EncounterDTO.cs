using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices.DTOs
{
    public class EncounterDTO
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public List<Guid> CommentsIds { get; set; } = new List<Guid>();

        public string SportName { get; set; }
    }
}
