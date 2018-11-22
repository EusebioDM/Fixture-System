using System.Collections.Generic;
using SilverFixture.IServices.DTOs;

namespace SilverFixture.IServices.Services_Interfaces
{
    public interface IEncounterSimpleServices
    {
        void AddComment(string encounterId, string comment);

        EncounterDTO CreateEncounter(EncounterDTO encounterDTO);

        void CreateEncounter(IEnumerable<EncounterDTO> encounterDTOs);

        void UpdateEncounter(EncounterDTO encounterModel);

        void DeleteEncounter(string id);

        IEnumerable<EncounterDTO> GetAllEncounters();

        EncounterDTO GetEncounter(string encounterId);
    }
}