using EirinDuran.IServices.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EirinDuran.WebApi.Models
{
    public class EncounterModelIn
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string HomeTeamName { get; set; }
        [Required]
        public string AwayTeamName { get; set; }
        [Required]
        public string SportName { get; set; }

        public override bool Equals(object obj)
        {
            var @in = obj as EncounterModelIn;
            return @in != null &&
                   Id.Equals(@in.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public EncounterDTO ToServicesDTO()
        {
            return new EncounterDTO()
            {
                Id = Id,
                DateTime = DateTime,
                HomeTeamName = HomeTeamName,
                AwayTeamName = AwayTeamName,
                SportName = SportName
            };
        }
    }
}
