using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Services_Interfaces;

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
