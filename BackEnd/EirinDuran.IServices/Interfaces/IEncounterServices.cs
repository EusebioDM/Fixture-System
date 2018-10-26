using EirinDuran.IServices.DTOs;
using System;
using System.Collections.Generic;

namespace EirinDuran.IServices.Interfaces
{
    public interface IEncounterServices
    {
        void AddComment(string encounterId, string comment);

        EncounterDTO CreateEncounter(EncounterDTO encounterDTO);

        void CreateEncounter(IEnumerable<EncounterDTO> encounterDTOs);

        void UpdateEncounter(EncounterDTO encounterModel);

        void DeleteEncounter(string id);

        IEnumerable<EncounterDTO> GetAllEncounters();

        EncounterDTO GetEncounter(string encounterId);

        IEnumerable<EncounterDTO> GetAllEncountersWithFollowedTeams();

        IEnumerable<CommentDTO> GetAllCommentsToOneEncounter(string encounterId);

        IEnumerable<EncounterDTO> GetEncountersBySport(string sportId);

        IEnumerable<EncounterDTO> GetEncountersByTeam(string teamId);

        IEnumerable<EncounterDTO> GetEncountersByDate(DateTime start, DateTime end);

        IEnumerable<EncounterDTO> CreateFixture(string fixtureGeneratorName, string sportName, DateTime startDate);

        IEnumerable<string> GetAvailableFixtureGenerators();
    }
}