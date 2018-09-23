using System.Collections.Generic;
using EirinDuran.Domain.Fixture;

namespace EirinDuran.IServices
{
    public interface IEncounterServices
    {
        void AddComment(Encounter encounterToComment, string comment);
        void CreateEncounter(EncounterDTO encounterDTO);
        void CreateEncounter(IEnumerable<EncounterDTO> encounterDTOs);
        void DeleteEncounter(Encounter encounter);
        IEnumerable<Encounter> GetAllEncounters();
        IEnumerable<Encounter> GetAllEncounters(Team team);
        IEnumerable<Encounter> GetAllEncountersWithFollowedTeams();
    }
}