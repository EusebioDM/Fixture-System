using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices
{
    public class EncounterDTO
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public List<Guid> CommentsIds { get; set; }

        public string SportName { get; set; }
    }
}
