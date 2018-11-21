using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SilverFixture.IServices.DTOs;
using SilverFixture.IServices.Exceptions;
using SilverFixture.IServices.Services_Interfaces;

namespace EirinDuran.WebApi.Models
{
    public class EncounterUpdateModelIn
    {
        public Guid Id { get; set; }
        
        public DateTime DateTime { get; set; }
        
        public List<TeamResult> Results { get; set; } = new List<TeamResult>();


        public void UpdateServicesDTO(EncounterDTO servicesDTO, ITeamServices teamServices)
        {
            try
            {
                UpdateDateTimeIfNecessary(servicesDTO);
                UpdateResultsIfNecessary(servicesDTO, teamServices);
            }
            catch (ArgumentException ex)
            {
                throw new ServicesException("There are teams repeated in the results");
            }
        }

        private void UpdateResultsIfNecessary(EncounterDTO servicesDTO, ITeamServices teamServices)
        {
            if (!Results.Any()) return;
            Dictionary<TeamDTO, int> results = new Dictionary<TeamDTO, int>();
            foreach (TeamResult result in Results)
            {
                TeamDTO team = teamServices.GetTeam(result.TeamId);
                results.Add(team, result.Result);
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