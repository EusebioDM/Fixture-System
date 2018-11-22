using SilverFixture.IServices.DTOs;

namespace SilverFixture.IServices.Services_Interfaces
{
    public interface ILoginServices
    {
        void CreateSession(string userName, string password);

        UserDTO LoggedUser { get; }
    }
}
