using SilverFixture.IServices.DTOs;
using SilverFixture.IServices.Services_Interfaces;

namespace EirinDuran.WebApiTest
{
    public class LoginServicesMock : ILoginServices
    {
        public LoginServicesMock(UserDTO user)
        {
            this.LoggedUser = user;
        }

        public UserDTO LoggedUser { get; }

        public void CreateSession(string userName, string password)
        {
        }
    }
}
