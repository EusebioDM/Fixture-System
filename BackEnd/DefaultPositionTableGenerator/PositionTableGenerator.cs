using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EirinDuran.Domain.Fixture;

namespace DefaultPositionTableGenerator
{
    public class PositionTableGenerator : IPositionTableGenerator
    {
        private Dictionary<string, int> positions;


        public Dictionary<string, int> GetPositionTable(IEnumerable<Encounter> encounters)
        {
            positions = new Dictionary<string, int>();

            foreach (Encounter encounter in encounters)
            {
                AddEncounterResultsToPositions(encounter);
            }

            return positions;
        }

        private void AddEncounterResultsToPositions(Encounter encounter)
        {
            if (encounter.Results.Any())
            {
                if (encounter.Sport.EncounterPlayerCount == EncounterPlayerCount.TwoPlayers)
                    AddTwoPlayerEncounterResults(encounter.Results);
                else
                    AddMoreThanTwoPlayerEncounterResults(encounter.Results);
            }
        }

        private void AddTwoPlayerEncounterResults(IEnumerable<KeyValuePair<Team,int>> encounterResults)
        {   
            Debug.Assert(encounterResults.Count() == 2);
            KeyValuePair<Team, int> first = encounterResults.OrderBy(p => p.Value).First();
            KeyValuePair<Team, int> second = encounterResults.OrderBy(p => p.Value).Last();

            if (first.Value == second.Value)
            {
                AddOrUpdateResult(first.Key.Name, 1);
                AddOrUpdateResult(second.Key.Name, 1);
            }
            else
            {
                AddOrUpdateResult(first.Key.Name, 3);
                AddOrUpdateResult(second.Key.Name, 0);
            }


        }

        private void AddMoreThanTwoPlayerEncounterResults(IEnumerable<KeyValuePair<Team,int>> encounterResults)
        {
            List<KeyValuePair<Team, int>> resultsCopy = encounterResults.OrderBy(p => p.Value).ToList();

            for (int i = 0; i < resultsCopy.Count(); i++)
            {
                switch (i)
                {
                    case 0:
                        AddOrUpdateResult(resultsCopy[i].Key.Name, 3);
                        break;
                    case 1:
                        AddOrUpdateResult(resultsCopy[i].Key.Name, 2);
                        break;
                    case 2:
                        AddOrUpdateResult(resultsCopy[i].Key.Name, 1);
                        break;
                    default:
                        AddOrUpdateResult(resultsCopy[i].Key.Name, 0);
                        break;
                }
            }        
        }

        private void AddOrUpdateResult(string teamName, int result)
        {
            if (positions.ContainsKey(teamName))
            {
                positions[teamName] = positions[teamName] + result;
            }
            else
            {
                positions[teamName] = result;
            }
        }
    }
}