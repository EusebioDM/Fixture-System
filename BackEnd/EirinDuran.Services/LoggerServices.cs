using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Services_Interfaces;
using EirinDuran.Services.DTO_Mappers;

namespace EirinDuran.Services
{
    public class LoggerServices : ILoggerServices
    {
        private readonly IRepository<LogDTO> repo;
        private readonly PermissionValidator validator;
        
       public LoggerServices(ILoginServices login, IRepository<LogDTO> repo)
        {
            this.repo = repo;
            validator = new PermissionValidator(Role.Administrator, login);
        }

        public IEnumerable<LogDTO> GetLogs(DateTime start, DateTime end)
        {
            validator.ValidatePermissions();
            List<LogDTO> logs = repo.GetAll().ToList();
            if (start != new DateTime())
                logs.RemoveAll(l => l.DateTime < start);
            if (end != new DateTime())
                logs.RemoveAll(l => l.DateTime > end);
            return logs;
        }
    }
}