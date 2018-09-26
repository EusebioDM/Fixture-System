using System.Collections.Generic;
using EirinDuran.Domain.Fixture;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Interfaces
{
    public interface IEncounterServices
    {
        void AddComment(Encounter encounterToComment, string comment);
        void CreateEncounter(EncounterDTO encounterDTO);
        void CreateEncounter(IEnumerable<EncounterDTO> encounterDTOs);
        void DeleteEncounter(string id);
        IEnumerable<Encounter> GetAllEncounters();
        IEnumerable<Encounter> GetAllEncounters(Team team);
        IEnumerable<Encounter> GetAllEncountersWithFollowedTeams();
    }
}