using EirinDuran.Domain.User;
using EirinDuran.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.ServicesTest
{
    public class LoginSericesMock : ILoginServices
    {
        public LoginSericesMock(User user)
        {
            this.LoggedUser = user;
        }

        public User LoggedUser { get; }

        public void CreateSession(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
