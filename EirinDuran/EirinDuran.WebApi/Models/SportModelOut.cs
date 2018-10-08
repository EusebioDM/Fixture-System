using EirinDuran.IServices.DTOs;
using System;

namespace EirinDuran.WebApi.Models
{
    public class SportModelOut
    {
        public string Name { get; set; }

        public string GetEncounters { get; set; }

        public SportModelOut(SportDTO sport)
        {
            Name = sport.Name;
            GetEncounters = "/api/sports/" + Name + "/ecounters";
        }

        public override bool Equals(object obj)
        {
            var @out = obj as SportModelOut;
            return @out != null &&
                   Name == @out.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}