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
