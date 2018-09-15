using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EirinDuran.DataAccessTest
{
    [TestClass]
    public class EncounterRepositoryTest
    {
        private EncounterRepository repo;
        private Sport futbol;
        private Sport rugby;
        private Team boca;
        private Team river;
        private Team tomba;
        private Encounter bocaRiver;
        private Encounter tombaRiver;


        [TestInitialize]
        public void TestInit()
        {
            repo = new EncounterRepository(GetContextFactory());
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            tomba = CreateGodoyCruzTeam();
            futbol = CreateFutbolTeam();
            rugby = CreateRugbyTeam();
            bocaRiver = CreateBocaRiverEncounter();
            tombaRiver = CreateTombaRiverEncounter();
            repo.Add(bocaRiver);
            repo.Add(tombaRiver);
        }

        private IContextFactory GetContextFactory()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).EnableSensitiveDataLogging().Options;
            return new ContextFactory(options);
        }

        private Sport CreateFutbolTeam()
        {
            Sport futbol = new Sport("Futbol");
            futbol.AddTeam(boca);
            futbol.AddTeam(river);
            return futbol;
        }

        private Sport CreateRugbyTeam()
        {
            return new Sport("Rugby");
        }

        private Team CreateBocaTeam()
        {
            string name = "Boca Juniors";
            Image image = Image.FromFile("..\\..\\..\\Resources\\Boca.jpg");
            return new Team(name, image);

        }

        private Team CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            Image image = Image.FromFile("..\\..\\..\\Resources\\River.jpg");
            return new Team(name, image);
        }

        private Team CreateGodoyCruzTeam()
        {
            string name = "River Plate";
            Image image = Image.FromFile("..\\..\\..\\Resources\\GodoyCruz.jpg");
            return new Team(name, image);
        }

        private Encounter CreateTombaRiverEncounter()
        {
            return new Encounter(futbol, new List<Team>() { boca, river }, new DateTime(3001, 10, 10));
        }

        private Encounter CreateBocaRiverEncounter()
        {
            return new Encounter(futbol, new List<Team>() { tomba, river }, new DateTime(3001, 10, 11));
        }
    }
}
