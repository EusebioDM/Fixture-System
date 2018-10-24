using System;
using System.Collections.Generic;
using EirinDuran.DataAccess.Entities;
using EirinDuran.GenericEntityRepository;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EirinDuran.DataAccess
{
    public class LogRepository : IRepository<Log>
    {
        private EntityRepository<Log, Log> repo;

        public LogRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            Func<Log> createLog = () => new Log();
            EntityFactory<Log> logFactory = new EntityFactory<Log>(createLog);
            Func<DbContext, DbSet<Log>> getDbSet = context => ((Context) context).Logs;
            repo = new EntityRepository<Log, Log>(logFactory, getDbSet, contextFactory);
        }

        public Log Get(string id) => repo.Get(Guid.Parse(id));

        public IEnumerable<Log> GetAll() => repo.GetAll();

        public void Add(Log model) => repo.Add(model);

        public void Update(Log model) => repo.Update(model);

        public void Delete(string id) => repo.Delete(Guid.Parse(id));
    }
}