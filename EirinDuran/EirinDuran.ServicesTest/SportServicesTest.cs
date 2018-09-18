using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.Services;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class SportServicesTest
    {
        private ILoginServices login;
        private SportRepository repo;
        private Sport futbol;
        private Sport rugby;
        private Team boca;
        private Team river;


        [TestMethod]
        public void CreatedSportTest()
        {
            SportServices service = new SportServices(login, repo);
            service.Create(rugby);

            Assert.IsTrue(repo.GetAll().Contains(rugby));
        }

        [TestInitialize]
        public void TestInit()
        {
            repo = new SportRepository(GetContextFactory());
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            futbol = CreateFutbolTeam();
            rugby = CreateRugbyTeam();
            repo.Add(futbol);
            login = CreateLoginServices();
        }

        private ILoginServices CreateLoginServices()
        {
            return new LoginSericesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
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

    }


}
