using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    public class SportRepository : IRepository<Sport>
    {
        private EntityRepository<Sport, SportEntity> repo;
        private IContext context;

        public SportRepository(IContext context)
        {
            EntityFactory<SportEntity> factory = CreateEntityFactory();
            Func<IContext, DbSet<SportEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<Sport, SportEntity>(factory, dbSet, context);
        }

        private EntityFactory<SportEntity> CreateEntityFactory() => new EntityFactory<SportEntity>(() => new SportEntity());

        private Func<IContext, DbSet<SportEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => c.Sports;

        public void Add(Sport model) => repo.Add(model);

        public void Delete(Sport model) => repo.Delete(model);

        public Sport Get(string id) => repo.Get(id);

        public IEnumerable<Sport> GetAll() => repo.GetAll();

        public void Update(Sport model) => repo.Update(model);
    }
}
