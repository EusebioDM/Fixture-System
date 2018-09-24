using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using EirinDuran.Entities.Mappers;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    public class TeamRepository : IRepository<Team>
    {
        private EntityRepository<Team, TeamEntity> repo;

        public TeamRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            EntityFactory<TeamEntity> Entityfactory = CreateEntityFactory();
            Func<Context, DbSet<TeamEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<Team, TeamEntity>(Entityfactory, dbSet, contextFactory);
        }

        private EntityFactory<TeamEntity> CreateEntityFactory() => new EntityFactory<TeamEntity>(() => new TeamEntity());

        private Func<Context, DbSet<TeamEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => c.Teams;

        public void Add(Team id) => repo.Add(id);

        public void Delete(string id) => repo.Delete(id);

        public Team Get(string id) => repo.Get(id);

        public IEnumerable<Team> GetAll() => repo.GetAll();

        public void Update(Team id) => repo.Update(id);
    }
}
