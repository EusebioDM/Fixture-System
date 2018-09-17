using System;
using EirinDuran.Domain.User;

namespace EirinDuran.Services
{
    public class LoginServices
    {
        public void CreateSession(string userName, string password)
        {
            LoggedUser = new User();
            LoggedUser.UserName = userName;
        }

        public User LoggedUser { get; private set; }
    }
}
