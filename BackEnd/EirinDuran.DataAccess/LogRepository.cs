using System;
using System.Collections.Generic;
using EirinDuran.DataAccess.Entities;
using EirinDuran.GenericEntityRepository;
using SilverFixture.IDataAccess;
using SilverFixture.IServices.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EirinDuran.DataAccess
{
    public class LogRepository : IRepository<LogDTO>
    {
        private EntityRepository<LogDTO, LogEntity> repo;

        public LogRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            Func<LogEntity> createLog = () => new LogEntity();
            EntityFactory<LogEntity> logFactory = new EntityFactory<LogEntity>(createLog);
            Func<DbContext, DbSet<LogEntity>> getDbSet = context => ((Context) context).Logs;
            repo = new EntityRepository<LogDTO, LogEntity>(logFactory, getDbSet, contextFactory);
        }

        public LogDTO Get(string id) => repo.Get(Guid.Parse(id));

        public IEnumerable<LogDTO> GetAll() => repo.GetAll();

        public void Add(LogDTO model) => repo.Add(model);

        public void Update(LogDTO model) => repo.Update(model);

        public void Delete(string id) => repo.Delete(Guid.Parse(id));
    }
}