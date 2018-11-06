using EirinDuran.DataAccess.Entities;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.Infrastructure_Interfaces;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.WebApi.Controllers
{
    public class DataBaseLogger : ILogger
    {
        private IRepository<Log> repo;
        
        public DataBaseLogger(IRepository<Log> repo)
        {
            this.repo = repo;
        } 
        
        public void Log(string userName, string action)
        {
            Log log = new Log(userName,action);
            repo.Add(log);
        }
    }
}