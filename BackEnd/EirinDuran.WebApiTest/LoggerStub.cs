
using EirinDuran.Domain.User;
using EirinDuran.IServices.Infrastructure_Interfaces;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.WebApiTest
{
    internal class LoggerStub : ILogger
    {
        public void Log(string userName, string action)
        {
            
        }
    }
}