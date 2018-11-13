﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices.DTOs
{
    public class EncounterDTO
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public List<Guid> CommentsIds { get; set; } = new List<Guid>();

        public string SportName { get; set; }

        public override bool Equals(object obj)
        {
            var dTO = obj as EncounterDTO;
            return dTO != null &&
                   Id.Equals(dTO.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        }
    }
}