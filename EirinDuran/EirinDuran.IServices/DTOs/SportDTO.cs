using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices.DTOs
{
    public class SportDTO
    {
        public string Name { get; set; }

        public List<string> TeamsNames { get; set; } = new List<string>();
    }
}
