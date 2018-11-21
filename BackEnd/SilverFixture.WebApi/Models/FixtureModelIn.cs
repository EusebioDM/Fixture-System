using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilverFixture.WebApi.Models
{
    public class FixtureModelIn
    {
        public string CreationAlgorithmName { get; set; }
        public string SportName { get; set; }
        public DateTime StartingDate { get; set; }
    }
}
