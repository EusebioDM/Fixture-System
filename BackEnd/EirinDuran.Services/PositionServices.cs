using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Infrastructure_Interfaces;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.Services
{
    public class PositionServices : IPositionsServices
    {
        private const string ResultsGeneratorFolderName = "PositionTableGenerators";
        private readonly IExtendedEncounterRepository encounterRepo;
        private readonly IAssemblyLoader assemblyLoader;

        public PositionServices(IExtendedEncounterRepository encounterRepo, IAssemblyLoader assemblyLoader)
        {
            this.encounterRepo = encounterRepo;
            this.assemblyLoader = assemblyLoader;
            SetupAssemblyLoader();
        }

        public IReadOnlyDictionary<string, int> GetPositionsTable(string sport)
        {
            try
            {
                return TryToGetPositionsTable(sport);
            }
            catch (InvalidOperationException)
            {
                throw new ServicesException($"No valid PositionGenerator was found in folder {ResultsGeneratorFolderName}");
            }
        }

        private IReadOnlyDictionary<string, int> TryToGetPositionsTable(string sport)
        {
            IPositionTableGenerator generator = assemblyLoader.GetImplementations<IPositionTableGenerator>().First();
            IEnumerable<Encounter> encounters = encounterRepo.GetBySport(sport);
            return generator.GetPositionTable(encounters);
        }

        private void SetupAssemblyLoader()
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string generatorFolder = Directory.GetDirectories(currentDir).First(d => d.EndsWith(ResultsGeneratorFolderName));
            assemblyLoader.AssembliesPath = generatorFolder;
        }
    }
}