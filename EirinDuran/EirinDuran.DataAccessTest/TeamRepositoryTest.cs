using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
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
            private TeamRepository repo;
            private Team boca;
            private Team river;

            [TestMethod]
            public void AddTeamTest()
            {
                repo.Add(boca);
                repo.Add(river);

                IEnumerable<Team> actual = repo.GetAll();
                IEnumerable<Team> expected = new List<Team>() { boca, river };

                Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
            }

            [TestMethod]
            [ExpectedException(typeof(ObjectAlreadyExistsInDataBaseException))]
            public void AddExistingTeamTest()
            {
                repo.Add(boca);
                repo.Add(boca);
            }

            [TestMethod]
            public void RemoveTeamTest()
            {
                repo.Add(boca);
                repo.Add(river);
                repo.Delete(river);

                IEnumerable<Team> actual = repo.GetAll();
                IEnumerable<Team> expected = new List<Team>() { boca };

                Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
            }

            [TestMethod]
            [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
            public void RemoveNonExistingTeamTest()
            {
                repo.Delete(boca);
            }

            [TestMethod]
            public void GetTeamTest()
            {
                repo.Add(boca);
                Team fromRepo = repo.Get(boca.Name);

                Assert.AreEqual(boca.Name, fromRepo.Name);
                Assert.IsTrue(ImagesAreTheSame(boca.Logo, fromRepo.Logo));
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
                repo.Add(boca);
                boca.Logo = river.Logo;
                repo.Update(boca);
                boca = CreateBocaTeam();

                Team fromRepo = repo.Get(boca.Name);
                Assert.IsFalse(ImagesAreTheSame(boca.Logo, fromRepo.Logo));
            }

            [TestMethod]
            [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
            public void UpdateNonExistantTeamTest()
            {
                repo.Update(boca);
            }

            private bool ImagesAreTheSame(Image first, Image second)
            {
                byte[] firstImageBytes = GetImageBytes(first);
                byte[] secondImageBytes = GetImageBytes(second);

                return Enumerable.SequenceEqual(firstImageBytes, secondImageBytes);
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
                repo = new TeamRepository(GetTestContext());
                boca = CreateBocaTeam();
                river = CreateTeamThatBelongsInTheB();
                CleanUpRepo();
            }

            private IContext GetTestContext()
            {
                DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("In Memory Test DB").Options;
                return new Context(options);
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

            private void CleanUpRepo()
            {
                foreach (Team team in repo.GetAll())
                    repo.Delete(team);
            }
        }
    }
}
