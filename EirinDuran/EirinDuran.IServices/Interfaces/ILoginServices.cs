using EirinDuran.Domain.User;
using System;

namespace EirinDuran.IServices.Interfaces
{
    public interface ILoginServices
    {
        void CreateSession(string userName, string password);

        User LoggedUser { get; }
    }
}
