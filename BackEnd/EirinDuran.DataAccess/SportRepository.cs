using EirinDuran.Domain.Fixture;
using EirinDuran.DataAccess;
using SilverFixture.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EirinDuran.GenericEntityRepository;
using EirinDuran.DataAccess.Entities;

namespace EirinDuran.DataAccess
{
    public class SportRepository : IRepository<Sport>
    {
        private EntityRepository<Sport, SportEntity> repo;

        public SportRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            EntityFactory<SportEntity> factory = CreateEntityFactory();
            Func<DbContext, DbSet<SportEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<Sport, SportEntity>(factory, dbSet, contextFactory);
        }

        private EntityFactory<SportEntity> CreateEntityFactory() => new EntityFactory<SportEntity>(() => new SportEntity());

        private Func<DbContext, DbSet<SportEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => ((Context)c).Sports;

        public void Add(Sport sport) => repo.Add(sport);

        public void Delete(string id) => repo.Delete(id);

        public Sport Get(string id) => repo.Get(id);

        public IEnumerable<Sport> GetAll() => repo.GetAll();

        public void Update(Sport sport) => repo.Update(sport);
    }
}
