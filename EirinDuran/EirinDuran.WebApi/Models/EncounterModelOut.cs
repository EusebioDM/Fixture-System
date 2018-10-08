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
     
        public string HomeTeamName { get; set; }
     
        public string AwayTeamName { get; set; }
     
        public string SportName { get; set; }

        public string CommentariesUrl { get; set; }

        public string AddCommentariesUrl { get; set; }

        public string GetAvailableFixturesGeneratorsUrl { get; set; }

        public string SetAvailableFixturesGeneratorsUrl { get; set; }


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

        public EncounterModelOut(EncounterDTO encounter)
        {
            Id = encounter.Id;
            DateTime = encounter.DateTime;
            HomeTeamName = encounter.HomeTeamName;
            AwayTeamName = encounter.AwayTeamName;
            SportName = encounter.SportName;
           
            CommentariesUrl = Id + "/commentaries";
            AddCommentariesUrl = Id + "/commentaries";
            GetAvailableFixturesGeneratorsUrl = "fixture";
            SetAvailableFixturesGeneratorsUrl = "fixture";
        }
    }
}