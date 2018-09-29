using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    public class SportRepository : IRepository<Sport>
    {
        private EntityRepository<Sport, SportEntity> repo;

        public SportRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            EntityFactory<SportEntity> factory = CreateEntityFactory();
            Func<Context, DbSet<SportEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<Sport, SportEntity>(factory, dbSet, contextFactory);
        }

        private EntityFactory<SportEntity> CreateEntityFactory() => new EntityFactory<SportEntity>(() => new SportEntity());

        private Func<Context, DbSet<SportEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => c.Sports;

        public void Add(Sport id) => repo.Add(id);

        public void Delete(string id) => repo.Delete(id);

        public Sport Get(string id) => repo.Get(id);

        public IEnumerable<Sport> GetAll() => repo.GetAll();

        public void Update(Sport id) => repo.Update(id);
    }
}
