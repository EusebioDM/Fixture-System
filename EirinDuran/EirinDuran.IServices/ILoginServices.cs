using EirinDuran.Domain.User;
using System;

namespace EirinDuran.IServices
{
    public interface ILoginServices
    {
        void CreateSession(string userName, string password);

        User LoggedUser { get; }
    }
}
