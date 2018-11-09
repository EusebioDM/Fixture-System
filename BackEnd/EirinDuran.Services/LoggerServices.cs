using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.DataAccess.Entities;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Services_Interfaces;
using EirinDuran.Services.DTO_Mappers;

namespace EirinDuran.Services
{
    public class LoggerServices : ILoggerServices
    {
        private IRepository<Log> repo;
        private PermissionValidator validator;
        private LogMapper mapper;
        
       public LoggerServices(ILoginServices login, IRepository<Log> repo)
        {
            this.repo = repo;
            validator = new PermissionValidator(Role.Administrator, login);
            mapper = new LogMapper();
        }

        public IEnumerable<LogDTO> GetLogs(DateTime start, DateTime end)
        {
            validator.ValidatePermissions();
            List<Log> logs = repo.GetAll().ToList();
            if (start != new DateTime())
                logs.RemoveAll(l => l.DateTime < start);
            if (end != new DateTime())
                logs.RemoveAll(l => l.DateTime > end);
            return logs.Select(l => mapper.Map(l));
        }
    }
}