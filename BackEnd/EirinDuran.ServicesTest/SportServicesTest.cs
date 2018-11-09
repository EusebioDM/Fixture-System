using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.Services;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class SportServicesTest
    {
        private IDesignTimeDbContextFactory<Context> contextFactory;
        private ILoginServices login;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private SportDTO futbol;
        private SportDTO rugby;
        private TeamDTO boca;
        private TeamDTO river;


        [TestMethod]
        public void CreatedSportTest()
        {
            SportServices service = new SportServices(login, sportRepo);
            service.CreateSport(rugby);

            Assert.IsTrue(sportRepo.GetAll().Any(s => s.Name.Equals(rugby.Name)));
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void CreateAlreadyExistingSport()
        {
            SportServices service = new SportServices(login, sportRepo);
            service.CreateSport(futbol);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void CreateInvalidNullSportTest()
        {
            SportServices service = new SportServices(login, sportRepo);
            SportDTO sport = new SportDTO();
            service.CreateSport(sport);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void CreateInvalidNameSportTest()
        {
            SportServices service = new SportServices(login, sportRepo);
            SportDTO sport = new SportDTO()
            {
                Name = "                  "
            };
            service.CreateSport(sport);
        }

        [TestInitialize]
        public void TestInit()
        {
            contextFactory = GetContextFactory();
            sportRepo = new SportRepository(contextFactory);
            teamRepo = new TeamRepository(contextFactory);
            futbol = CreateFutbol();
            rugby = CreateRugby();
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            sportRepo.Add(new Sport("Futbol"));
            teamRepo.Add(new Team(boca.Name, new Sport("Futbol")));
            teamRepo.Add(new Team(river.Name, new Sport("Futbol")));
            login = CreateLoginServices();
        }

        private ILoginServices CreateLoginServices()
        {
            return new LoginServicesMock(new UserDTO()
            {
                UserName = "Macri",
                Name = "Mauricio",
                Surname = "Macri",
                Password = "cat123",
                Mail = "mail@gmail.com",
                IsAdmin = true
            });
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
        }


        private TeamDTO CreateBocaTeam()
        {
            string name = "Boca Juniors";
            Image image = Image.FromFile(GetResourcePath("Boca.jpg"));
            return new TeamDTO()
            {
                Name = name,
                Logo = EncondeImage(image)
            };

        }

        private TeamDTO CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            Image image = Image.FromFile(GetResourcePath("River.jpg"));
            return new TeamDTO()
            {
                Name = name,
                Logo = EncondeImage(image)
            };
        }

        private SportDTO CreateFutbol()
        {
            return new SportDTO()
            {
                Name = "Futbol"
            };
        }

        private SportDTO CreateRugby()
        {
            return new SportDTO()
            {
                Name = "Rugby"
            };
        }

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
        }

        private string EncondeImage(Image image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imageBytes = stream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
    }
}
