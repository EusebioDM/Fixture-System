using System.Collections.Generic;
using EirinDuran.Domain.Fixture;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Interfaces
{
    public interface IEncounterServices
    {
        void AddComment(string encounterId, string comment);
        void CreateEncounter(EncounterDTO encounterDTO);
        void CreateEncounter(IEnumerable<EncounterDTO> encounterDTOs);
        void DeleteEncounter(string id);
        IEnumerable<EncounterDTO> GetAllEncounters();
        IEnumerable<Encounter> GetAllEncounters(Team team);
        IEnumerable<Encounter> GetAllEncountersWithFollowedTeams();
        IEnumerable<Comment> GetAllCommentsToOneEncounter(string encounterId);
    }
}