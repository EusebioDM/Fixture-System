using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.WebApi.Models
{
    public class EncounterUpdateModelIn
    {
        public Guid Id { get; set; }
        
        public DateTime DateTime { get; set; }
        
        public Dictionary<string, int> Results { get; set; } = new Dictionary<string, int>();


        public void UpdateServicesDTO(EncounterDTO servicesDTO, ITeamServices teamServices)
        {
            UpdateDateTimeIfNecessary(servicesDTO);
            UpdateResultsIfNecessary(servicesDTO, teamServices);
        }

        private void UpdateResultsIfNecessary(EncounterDTO servicesDTO, ITeamServices teamServices)
        {
            if (!Results.Any()) return;
            Dictionary<TeamDTO, int> results = new Dictionary<TeamDTO, int>();
            foreach (KeyValuePair<string, int> pair in Results)
            {
                TeamDTO team = teamServices.GetTeam(pair.Key + "_" + servicesDTO.SportName);
                results.Add(team, pair.Value);
            }

            servicesDTO.Results = results;
        }

        private void UpdateDateTimeIfNecessary(EncounterDTO servicesDTO)
        {
            if (DateTime != new DateTime())
                servicesDTO.DateTime = DateTime;
        }
    }
}