using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Entities
{
    public class SportEntity
    {
        public string Name { get; set; }
        public IEnumerable<TeamEntity> Teams { get; set; }
    }
}
