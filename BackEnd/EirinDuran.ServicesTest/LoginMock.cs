using EirinDuran.Domain.User;
using SilverFixture.IServices;
using SilverFixture.IServices.DTOs;
using SilverFixture.Services;
using System;
using System.Collections.Generic;
using System.Text;
using SilverFixture.IServices.Services_Interfaces;

namespace EirinDuran.ServicesTest
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
            throw new NotImplementedException();
        }
    }
}
