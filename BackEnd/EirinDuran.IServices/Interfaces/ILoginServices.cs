using EirinDuran.IServices.DTOs;
using System;

namespace EirinDuran.IServices.Interfaces
{
    public interface ILoginServices
    {
        void CreateSession(string userName, string password);

        UserDTO LoggedUser { get; }
    }
}
