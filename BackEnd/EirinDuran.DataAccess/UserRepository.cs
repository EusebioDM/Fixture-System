using EirinDuran.Domain;
using EirinDuran.Domain.User;
using EirinDuran.DataAccess;
using EirinDuran.DataAccess.Entities.Mappers;
using SilverFixture.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.GenericEntityRepository;
using EirinDuran.DataAccess.Entities;

namespace EirinDuran.DataAccess
{
    public class UserRepository : IRepository<User>
    {
        private EntityRepository<User, UserEntity> repo;

        public UserRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            EntityFactory<UserEntity> factory = CreateEntityFactory();
            Func<DbContext, DbSet<UserEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<User, UserEntity>(factory, dbSet, contextFactory);
        }

        private EntityFactory<UserEntity> CreateEntityFactory() => new EntityFactory<UserEntity>(() => new UserEntity());

        private Func<DbContext, DbSet<UserEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => ((Context)c).Users;

        public void Add(User user) => repo.Add(user);

        public void Delete(string id) => repo.Delete(id);

        public User Get(string id) => repo.Get(id);

        public IEnumerable<User> GetAll() => repo.GetAll();

        public void Update(User user) => repo.Update(user);
    }
}