using System.Collections.Generic;

namespace EirinDuran.Domain.Fixture
{
    public class Sport
    {
        private string name;
        private StringValidator validator;
        public string Name { get => name; set => SetNameIfValid(value); }
        public  EncounterPlayerCount EncounterPlayerCount { get; private set; }

        public Sport(string name, EncounterPlayerCount encounterPlayerCount = EncounterPlayerCount.TwoPlayers)
        {
            validator = new StringValidator();
            Name = name;
            EncounterPlayerCount = encounterPlayerCount;
        }

        private void SetNameIfValid(string value)
        {
            bool valid = validator.ValidateNotNullOrEmptyString(value);
            if (!valid)
            {
                throw new EmptyFieldException("Name");
            }
            else
            {
                name = value;
            }
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
