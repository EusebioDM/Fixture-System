using SilverFixture.IServices.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EirinDuran.WebApi.Models
{
    public class EncounterModelIn
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public ICollection<string> TeamIds { get; set; }
        [Required]
        public string SportName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is EncounterModelIn @in &&
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
                TeamIds =  TeamIds,
                SportName = SportName
            };
        }
    }
}
