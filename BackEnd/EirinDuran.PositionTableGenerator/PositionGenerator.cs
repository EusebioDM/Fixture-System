using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.Domain;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.Positions;

namespace EirinDuran.PositionTableGenerator
{
    public class PositionGenerator : IPositionGenerator
    {
        private IEnumerable<Team> teams;
        
        public IEnumerable<Results> GeneratePositions(IEnumerable<Team> teams)
        {
            if (!teams.Any())
            {
                return new List<Results>();
            }

            this.teams = teams;
            Sport sport = teams.First().Sport;

            switch (sport.EncounterPlayerCount)
            {
                case EncounterPlayerCount.TwoPlayers:
                    return GenerateTwoPlayerPositions();
                    break;
                case EncounterPlayerCount.MoreThanTwoPlayers:
                    return GenerateMoranTwoPlayerPositions();
                    break;
                default:
                    throw new DomainException("");
            }
        }

        private IEnumerable<Results> GenerateMoranTwoPlayerPositions()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Results> GenerateTwoPlayerPositions()
        {
            throw new NotImplementedException();
        }
    }
}