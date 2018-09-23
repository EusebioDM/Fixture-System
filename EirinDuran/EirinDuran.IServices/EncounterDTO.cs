using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices
{
    public class EncounterDTO
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public TeamDTO HomeTeam { get; set; }

        public TeamDTO AwayTeam { get; set; }

        public ICollection<CommentDTO> Comments { get; set; }

        public SportDTO Sport { get; set; }
    }
}
