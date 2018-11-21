using System;
using System.Collections.Generic;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Infrastructure_Interfaces;

namespace EirinDuran.Logger
{
    public class DataBaseLogger : ILogger
    {
        private readonly IRepository<LogDTO> repo;
        private readonly HashSet<string> actionsToIgnore = new HashSet<string>() { "Login"};
        
        public DataBaseLogger(IRepository<LogDTO> repo)
        {
            this.repo = repo;
        } 
        
        public void Log(string userName, string action)
        {
            if (actionsToIgnore.Contains(action)) return;
            
            LogDTO logEntity = new LogDTO()
            {
                DateTime = DateTime.Now,
                Action = action,
                UserName = userName
            };
            repo.Add(logEntity);
        }
    }
}