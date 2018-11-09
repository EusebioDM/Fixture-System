using System;
using System.Collections.Generic;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Services_Interfaces
{
    public interface IEncounterQueryServices
    {
        IEnumerable<EncounterDTO> GetAllEncountersWithFollowedTeams();

        IEnumerable<CommentDTO> GetAllCommentsToOneEncounter(string encounterId);

        IEnumerable<EncounterDTO> GetEncountersBySport(string sportId);

        IEnumerable<EncounterDTO> GetEncountersByTeam(string teamId);

        IEnumerable<EncounterDTO> GetEncountersByDate(DateTime start, DateTime end);
    }
}