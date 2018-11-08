using EirinDuran.Domain.User;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.Services;
using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.IServices.Services_Interfaces;

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
