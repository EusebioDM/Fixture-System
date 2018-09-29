using EirinDuran.Domain.User;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
