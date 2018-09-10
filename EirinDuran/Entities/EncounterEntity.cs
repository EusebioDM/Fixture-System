using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Entities
{
    public class EncounterEntity
    {
        public DateTime DateTime { get; set; }
        public SportEntity Sport { get; set; }
        public IEnumerable<TeamEntity> Teams { get; set; }
    }
}
