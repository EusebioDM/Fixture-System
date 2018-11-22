using System.Collections.Generic;
using System.Drawing;

namespace SilverFixture.IServices.DTOs
{
    public class TeamDTO
    {
        public string Name { get; set; }

        public string Logo { get; set; }

        public string SportName { get; set; }

        public override bool Equals(object obj)
        {
            var dTO = obj as TeamDTO;
            return dTO != null &&
                   Name == dTO.Name &&
                   SportName == dTO.SportName;
        }

        public override int GetHashCode()
        {
            var hashCode = -2076754524;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SportName);
            return hashCode;
        }
    }
}