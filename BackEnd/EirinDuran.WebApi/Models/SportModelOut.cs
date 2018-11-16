using EirinDuran.IServices.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace EirinDuran.WebApi.Models
{
    public class SportModelOut
    {
        public string Name { get; set; }
        public string GetEncounters { get; }
        public string EncounterPlayerCount { get; set; }

        public SportModelOut(SportDTO sport)
        {
            Name = sport.Name;
            GetEncounters = "/api/sports/" + Name + "/encounters";
            EncounterPlayerCount = sport.EncounterPlayerCount.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is SportModelOut @out &&
                   Name == @out.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}