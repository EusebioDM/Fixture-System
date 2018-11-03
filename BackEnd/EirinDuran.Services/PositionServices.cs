using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;

namespace EirinDuran.Services
{
    public class PositionServices : IPositionsServices
    {
        private const string ResultsGeneratorFolderName = "PositionTableGenerators";
        private readonly IExtendedEncounterRepository encounterRepo;

        public PositionServices(IExtendedEncounterRepository encounterRepo)
        {
            this.encounterRepo = encounterRepo;
        }

        public Dictionary<string, int> GetPositionsTable(SportDTO sportDto)
        {
            try
            {
                return TryToGetPositionsTable(sportDto);
            }
            catch (InvalidOperationException)
            {
                throw new ServicesException($"No valid PositionGenerator was found in folder {ResultsGeneratorFolderName}");
            }
        }

        private Dictionary<string, int> TryToGetPositionsTable(SportDTO sportDto)
        {
            AssemblyLoader.AssemblyLoader loader = GetAssemblyLoader();
            IPositionTableGenerator generator = loader.GetImplementations<IPositionTableGenerator>().First();
            IEnumerable<Encounter> encounters = encounterRepo.GetBySport(sportDto.Name);
            return generator.GetPositionTable(encounters);
        }

        private AssemblyLoader.AssemblyLoader GetAssemblyLoader()
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string generatorFolder = Directory.GetDirectories(currentDir).First(d => d.EndsWith(ResultsGeneratorFolderName));
            return new AssemblyLoader.AssemblyLoader(generatorFolder);
        }
    }
}