using System;
using System.Collections.Generic;
using SilverFixture.IServices.DTOs;

namespace SilverFixture.IServices.Services_Interfaces
{
    public interface IFixtureServices
    {
        IEnumerable<EncounterDTO> CreateFixture(string fixtureGeneratorName, string sportName, DateTime startDate);

        IEnumerable<string> GetAvailableFixtureGenerators();
    }
}