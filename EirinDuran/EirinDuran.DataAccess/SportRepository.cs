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

        public void Add(Sport model) => repo.Add(model);

        public void Delete(Sport model) => repo.Delete(model);

        public Sport Get(Sport sport) => repo.Get(sport);

        public IEnumerable<Sport> GetAll() => repo.GetAll();

        public void Update(Sport model) => repo.Update(model);
    }
}
