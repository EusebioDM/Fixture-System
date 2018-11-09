using System;
using System.Collections.Generic;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Services_Interfaces
{
    public interface IFixtureServices
    {
        IEnumerable<EncounterDTO> CreateFixture(string fixtureGeneratorName, string sportName, DateTime startDate);

        IEnumerable<string> GetAvailableFixtureGenerators();
    }
}