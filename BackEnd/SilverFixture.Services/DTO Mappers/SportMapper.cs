using EirinDuran.Domain.Fixture;
using SilverFixture.IDataAccess;
using SilverFixture.IServices;
using SilverFixture.IServices.DTOs;
using SilverFixture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EncounterPlayerCount = SilverFixture.IServices.DTOs.EncounterPlayerCount;

namespace SilverFixture.Services.DTO_Mappers
{
    internal class SportMapper : DTOMapper<Sport, SportDTO>
    {
        public override SportDTO Map(Sport sport)
        {
            return new SportDTO()
            {
                Name = sport.Name,
                EncounterPlayerCount =  (EncounterPlayerCount)sport.EncounterPlayerCount
            };
        }

        protected override Sport TryToMapModel(SportDTO sportDTO)
        {
            return new Sport(name: sportDTO.Name, encounterPlayerCount: (EirinDuran.Domain.Fixture.EncounterPlayerCount)sportDTO.EncounterPlayerCount);
        }
    }
}
