using EirinDuran.IServices.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EirinDuran.WebApi.Models
{
    public class EncounterModelOut
    {
        public Guid Id { get; set; }
     
        public DateTime DateTime { get; set; }
        
        public ICollection<string> TeamIds { get; set; }
        
        public Dictionary<string, int> Results { get; set; } = new Dictionary<string, int>();
     
        public string SportName { get; set; }

        public string CommentariesUrl { get; set; }

        public string AddCommentariesUrl { get; }

        public string GetAvailableFixturesGeneratorsUrl { get; }

        public string SetAvailableFixturesGeneratorsUrl { get; }


        public override bool Equals(object obj)
        {
            return obj is EncounterModelOut @in &&
                   Id.Equals(@in.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public EncounterModelOut(EncounterDTO encounter)
        {
            Id = encounter.Id;
            DateTime = encounter.DateTime;
            TeamIds = encounter.TeamIds;
            SportName = encounter.SportName;
            encounter.Results.ToList().ForEach(p => Results.Add(p.Key.Name, p.Value));
           
            CommentariesUrl = "/api/encounters/" + Id + "/commentaries";
            AddCommentariesUrl = "/api/encounters/" + Id + "/commentaries";
            GetAvailableFixturesGeneratorsUrl = "/api/encounters/fixture";
            SetAvailableFixturesGeneratorsUrl = "/api/encounters/fixture";
        }
    }
}