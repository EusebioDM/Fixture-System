using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using EirinDuran.Entities.Mappers;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    public class TeamRepository : IRepository<Team>
    {
        private EntityRepository<Team, TeamEntity> repo;

        public TeamRepository(IContextFactory contextFactory)
        {
            EntityFactory<TeamEntity> Entityfactory = CreateEntityFactory();
            Func<IContext, DbSet<TeamEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<Team, TeamEntity>(Entityfactory, dbSet, contextFactory);
        }

        private EntityFactory<TeamEntity> CreateEntityFactory() => new EntityFactory<TeamEntity>(() => new TeamEntity());

        private Func<IContext, DbSet<TeamEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => c.Teams;

        public void Add(Team model) => repo.Add(model);

        public void Delete(Team model) => repo.Delete(model);

        public Team Get(Team team) => repo.Get(team);

        public IEnumerable<Team> GetAll() => repo.GetAll();

        public void Update(Team model) => repo.Update(model);
    }
}
