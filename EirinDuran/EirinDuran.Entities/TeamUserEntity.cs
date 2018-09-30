using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Entities
{
    public class TeamUserEntity
    {
        public string TeamName { get; set; }

        public virtual TeamEntity Team { get; set; }

        public virtual UserEntity User { get; set; }

        public string UserName { get; set; }
    }
}
