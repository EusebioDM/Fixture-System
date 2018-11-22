
using SilverFixture.Domain.User;
using SilverFixture.IServices.Infrastructure_Interfaces;
using SilverFixture.IServices.Services_Interfaces;

namespace SilverFixture.WebApiTest
{
    internal class LoggerStub : ILogger
    {
        public void Log(string userName, string action)
        {
            
        }
    }
}