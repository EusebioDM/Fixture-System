
using EirinDuran.Domain.User;
using EirinDuran.IServices.Interfaces;

namespace EirinDuran.ServicesTest
{
    internal class LoggerStub : ILogger
    {
        public void Log(string userName, string action)
        {
            
        }
    }
}