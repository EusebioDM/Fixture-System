using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Services_Interfaces
{
    public interface ILoginServices
    {
        void CreateSession(string userName, string password);

        UserDTO LoggedUser { get; }
    }
}
