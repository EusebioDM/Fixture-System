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
    public class EncounterRepository : IRepository<Encounter>
    {
        private EntityRepository<Encounter, EncounterEntity> repo;

        public EncounterRepository(IContextFactory contextFactory)
        {
            EntityFactory<EncounterEntity> factory = CreateEntityFactory();
            Func<IContext, DbSet<EncounterEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<Encounter, EncounterEntity>(factory, dbSet, contextFactory);
        }

        private EntityFactory<EncounterEntity> CreateEntityFactory() => new EntityFactory<EncounterEntity>(() => new EncounterEntity());

        private Func<IContext, DbSet<EncounterEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => c.Encounters;

        public void Add(Encounter model) => repo.Add(model);

        public void Delete(Encounter model) => repo.Delete(model);

        public Encounter Get(Encounter encounter) => repo.Get(encounter);

        public IEnumerable<Encounter> GetAll() => repo.GetAll();

        public void Update(Encounter model) => repo.Update(model);
    }
}
