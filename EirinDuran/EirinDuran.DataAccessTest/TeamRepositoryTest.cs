using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccessTest
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

            private TeamRepository repo;

            [TestMethod]
            public void AddTeamTest()
            {
                IEnumerable<Team> actual = repo.GetAll();
                IEnumerable<Team> expected = new List<Team>() { CreateBocaTeam(), CreateTeamThatBelongsInTheB() };
                Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
            }

            [TestMethod]
            [ExpectedException(typeof(ObjectAlreadyExistsInDataBaseException))]
            public void AddExistingTeamTest()
            {
                repo.Add(GetBocaTeam());
            }

            [TestMethod]
            public void RemoveTeamTest()
            {
                repo.Delete(GetRiverTeam());

                IEnumerable<Team> actual = repo.GetAll();
                IEnumerable<Team> expected = new List<Team>() { CreateBocaTeam() };

                Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
            }

            [TestMethod]
            [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
            public void RemoveNonExistingTeamTest()
            {
                repo.Delete(CreateBocaTeam());
                repo.Delete(CreateBocaTeam());
            }

            [TestMethod]
            public void GetTeamTest()
            {
                Team fromRepo = repo.Get("Boca Juniors");

                Assert.AreEqual("Boca Juniors", fromRepo.Name);
                Assert.IsTrue(ImagesAreTheSame(Image.FromFile(bocaImagePath), fromRepo.Logo));
            }

            [TestMethod]
            [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
            public void GetNonExistantTeamTest()
            {
                repo.Get("Godoy Cruz");
            }

            [TestMethod]
            public void UpdateTeamTest()
            {
                Team boca = GetBocaTeam();
                boca.Logo = GetRiverTeam().Logo;
                repo.Update(boca);

                Team fromRepo = repo.Get(boca.Name);
                Assert.IsFalse(ImagesAreTheSame(Image.FromFile(bocaImagePath), fromRepo.Logo));
            }

            [TestMethod]
            public void UpdateNonExistantTeamTest()
            {
                Team godoyCruz = new Team("Godoy Cruz", Image.FromFile(tombaImagePath));
                repo.Update(godoyCruz);
                Team fromRepo = repo.Get("Godoy Cruz");

                Assert.IsTrue(ImagesAreTheSame(Image.FromFile(tombaImagePath), fromRepo.Logo));
            }

            private bool ImagesAreTheSame(Image first, Image second)
            {
                byte[] firstImageBytes = GetImageBytes(first);
                byte[] secondImageBytes = GetImageBytes(second);
                bool areTheSame = firstImageBytes.Length == secondImageBytes.Length;
                for (int i = 0; i < firstImageBytes.Length && areTheSame; i++)
                {
                    areTheSame &= firstImageBytes[i] == secondImageBytes[i];
                }
                return areTheSame;
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
                bocaImagePath = GetResourcePath("Boca.jpg");
                riverImagePath = GetResourcePath("River.jpg");
                tombaImagePath = GetResourcePath("GodoyCruz.jpg");
                repo = new TeamRepository(GetTestContext());
                repo.Add(CreateBocaTeam());
                repo.Add(CreateTeamThatBelongsInTheB());
            }

            private IDesignTimeDbContextFactory<Context> GetTestContext()
            {
                DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
                return new InMemoryContextFactory(options);
            }

            private Team CreateBocaTeam()
            {
                string name = "Boca Juniors";
                Image image = Image.FromFile(bocaImagePath);
                return new Team(name, image);
            }

            private Team CreateTeamThatBelongsInTheB()
            {
                string name = "River Plate";
                Image image = Image.FromFile(riverImagePath);
                return new Team(name, image);
            }

            private void CleanUpRepo()
            {
                foreach (Team team in repo.GetAll())
                    repo.Delete(team);
            }

            private Team GetBocaTeam() => repo.Get("Boca Juniors");
            private Team GetRiverTeam() => repo.Get("River Plate");

            private string GetResourcePath(string resourceName)
            {
                string current = Directory.GetCurrentDirectory();
                string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
                return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
            }
        }
    }
}
