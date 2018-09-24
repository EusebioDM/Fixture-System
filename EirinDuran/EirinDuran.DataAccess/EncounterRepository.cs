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
    public class EncounterRepository : IRepository<Encounter>
    {
        private EntityRepository<Encounter, EncounterEntity> repo;

        public EncounterRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            EntityFactory<EncounterEntity> factory = CreateEntityFactory();
            Func<Context, DbSet<EncounterEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<Encounter, EncounterEntity>(factory, dbSet, contextFactory);
        }

        private EntityFactory<EncounterEntity> CreateEntityFactory() => new EntityFactory<EncounterEntity>(() => new EncounterEntity());

        private Func<Context, DbSet<EncounterEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => c.Encounters;

        public void Add(Encounter id) => repo.Add(id);

        public void Delete(object id) => repo.Delete(id);

        public Encounter Get(object id) => repo.Get(id);

        public IEnumerable<Encounter> GetAll() => repo.GetAll();

        public void Update(Encounter id) => repo.Update(id);
    }
}
