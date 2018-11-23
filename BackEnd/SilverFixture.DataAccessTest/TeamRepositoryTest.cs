using SilverFixture.DataAccess;
using SilverFixture.Domain.Fixture;
using SilverFixture.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SilverFixture.DataAccessTest
{
    using Helper = HelperFunctions<Team>;
    public class TeamRepositoryTest
    {
        [TestClass]
        public class TeamEntityRepositoryTest
        {
            private string bocaImagePath;
            private string riverImagePath;
            private string tombaImagePath;
            private Sport football;
            private TeamRepository repo;

            [TestMethod]
            public void AddTeamTest()
            {
                IEnumerable<Team> actual = repo.GetAll();
                IEnumerable<Team> expected = new List<Team>() { CreateBocaTeam(), CreateTeamThatBelongsInTheB() };
                Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
            }

            [TestMethod]
            [ExpectedException(typeof(DataAccessException))]
            public void AddExistingTeamTest()
            {
                repo.Add(GetBocaTeam());
            }

            [TestMethod]
            public void RemoveTeamTest()
            {
                repo.Delete(GetRiverTeam().Name + "_" + "Football");

                IEnumerable<Team> actual = repo.GetAll();
                IEnumerable<Team> expected = new List<Team>() { CreateBocaTeam() };

                Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
            }

            [TestMethod]
            [ExpectedException(typeof(DataAccessException))]
            public void RemoveNonExistingTeamTest()
            {
                repo.Delete(CreateBocaTeam().Name + "_" + "Football");
                repo.Delete(CreateBocaTeam().Name + "_" + "Football");
            }

            [TestMethod]
            public void GetTeamTest()
            {
                Team fromRepo = repo.Get("Boca Juniors_Football");

                Assert.AreEqual("Boca Juniors", (string)fromRepo.Name);
                Assert.IsTrue(ImagesAreTheSame(Image.FromFile(bocaImagePath), fromRepo.Logo));
            }

            [TestMethod]
            [ExpectedException(typeof(DataAccessException))]
            public void GetNonExistantTeamTest()
            {
                repo.Get("Godoy Cruz"+ "_" + "Football");
            }

            [TestMethod]
            public void UpdateNonExistantTeamTest()
            {
                Team godoyCruz = new Team("Godoy Cruz", football, Image.FromFile(tombaImagePath));
                repo.Update(godoyCruz);
                Team fromRepo = repo.Get("Godoy Cruz_Football");
                Assert.IsTrue(ImagesAreTheSame(Image.FromFile(tombaImagePath), fromRepo.Logo));
            }

            [TestMethod]
            public void AddMultipleTeamsWithSameNameTest()
            {
                Team team = new Team("Boca Juniors", new Sport("NotFootball"));
                repo.Add(team);

                Team firstFromRepo = repo.Get("Boca Juniors_NotFootball");
                Team secondFromRepo = repo.Get("Boca Juniors_Football");
                Assert.AreEqual("Boca Juniors", (string)firstFromRepo.Name);
                Assert.AreEqual("NotFootball", (string)firstFromRepo.Sport.Name);
                Assert.AreEqual("Boca Juniors", (string)secondFromRepo.Name);
                Assert.AreEqual("Football", (string)secondFromRepo.Sport.Name);
            }

            private bool ImagesAreTheSame(Image first, Image second)
            {
                return first.Flags == second.Flags;
            }

            private byte[] GetImageBytes(Image image)
            {
                MemoryStream stream = new MemoryStream();
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }

            [TestInitialize]
            public void TestInit()
            {
                football = new Sport("Football");
                bocaImagePath = GetResourcePath("Boca.jpg");
                riverImagePath = GetResourcePath("River.jpg");
                tombaImagePath = GetResourcePath("GodoyCruz.jpg");
                repo = new TeamRepository(GetTestContext());
                repo.Add(CreateBocaTeam());
                repo.Add(CreateTeamThatBelongsInTheB());
            }

            private IDesignTimeDbContextFactory<Context> GetTestContext()
            {
                return new InMemoryContextFactory();
            }

            private Team CreateBocaTeam()
            {
                string name = "Boca Juniors";
                Image image = Image.FromFile(bocaImagePath);
                return new Team(name, football, image);
            }

            private Team CreateTeamThatBelongsInTheB()
            {
                string name = "River Plate";
                Image image = Image.FromFile(riverImagePath);
                return new Team(name, football, image);
            }

            private void CleanUpRepo()
            {
                foreach (Team team in repo.GetAll())
                    repo.Delete(team.Name);
            }

            private Team GetBocaTeam() => repo.Get("Boca Juniors_Football");
            private Team GetRiverTeam() => repo.Get("River Plate_Football");

            private string GetResourcePath(string resourceName)
            {
                string current = Directory.GetCurrentDirectory();
                string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
                return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
            }
        }
    }
}
