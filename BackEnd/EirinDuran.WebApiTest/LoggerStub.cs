
using EirinDuran.Domain.User;
using EirinDuran.IServices.Interfaces;

namespace EirinDuran.WebApiTest
{
    internal class LoggerStub : ILogger
    {
        public void Log(string userName, string action)
        {
            
        }
    }
}