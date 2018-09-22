using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class UsersControllerTest
    {
        [TestMethod]
        public void GetAllUsersOk()
        {
            //Arrange: Construimos el mock y seteamos las expectativas
            /*var expectedUsers = GetFakeUsers();
            var mockUserService = new Mock<IUserServices>();
            mockUserService
                .Setup(bl => bl.GetAll())
                .Returns(expectedUsers);
            var controller = new UsersController(mockUserService.Object);

            //Act
            var obtainedResult = controller.Get() as ActionResult<List<User>>;

            //Assert
            mockUserService.VerifyAll();
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(obtainedResult.Value, expectedUsers);*/
        }
    }
}
