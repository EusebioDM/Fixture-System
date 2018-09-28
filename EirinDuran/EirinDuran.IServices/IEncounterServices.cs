using EirinDuran.Domain.Fixture;
using System.Collections.Generic;

namespace EirinDuran.IServices
{
    public interface IEncounterServices
    {
        void CreateEncounter(Encounter encounter);

        void CreateEncounter(IEnumerable<Encounter> encounters);

        void AddComment(Encounter encounterToComment, string comment);

        IEnumerable<Encounter> GetAllEncounters();

        IEnumerable<Encounter> GetAllEncounters(Team team);

        void DeleteEncounter(Encounter encounter);

        IEnumerable<Encounter> GetAllEncountersWithFollowedTeams();
    }
}