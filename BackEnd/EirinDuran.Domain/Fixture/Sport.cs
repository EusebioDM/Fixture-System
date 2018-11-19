using System.Collections.Generic;

namespace EirinDuran.Domain.Fixture
{
    public class Sport
    {
        public Name Name { get; private set; }
        public  EncounterPlayerCount EncounterPlayerCount { get; private set; }

        public Sport(string name, EncounterPlayerCount encounterPlayerCount = EncounterPlayerCount.TwoPlayers)
        {
            Name = name;
            EncounterPlayerCount = encounterPlayerCount;
        }

        public override bool Equals(object obj)
        {
            return obj is Sport sport &&
                   Name == sport.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
