using SilverFixture.Domain.Fixture;
using SilverFixture.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EirinDuran.GenericEntityRepository;
using SilverFixture.DataAccess.Entities;

namespace SilverFixture.DataAccess
{
    public class TeamRepository : IRepository<Team>
    {
        private EntityRepository<Team, TeamEntity> repo;

        public TeamRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            EntityFactory<TeamEntity> Entityfactory = CreateEntityFactory();
            Func<DbContext, DbSet<TeamEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<Team, TeamEntity>(Entityfactory, dbSet, contextFactory);
        }

        private EntityFactory<TeamEntity> CreateEntityFactory() => new EntityFactory<TeamEntity>(() => new TeamEntity());

        private Func<DbContext, DbSet<TeamEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => ((Context)c).Teams;

        public void Add(Team team) => repo.Add(team);

        public void Delete(string ids)
        {
            string[] keys = ids.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            repo.Delete(keys);
        }

        public Team Get(string id)
        {
            string[] keys = id.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            return repo.Get(keys);
        }

        public IEnumerable<Team> GetAll() => repo.GetAll();

        public void Update(Team team) => repo.Update(team);
    }
}
