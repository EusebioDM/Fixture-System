using EirinDuran.Domain;
using EirinDuran.Domain.User;
using EirinDuran.Entities;
using EirinDuran.Entities.Mappers;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.DataAccess
{
    public class UserRepository : IRepository<User>
    {
        private EntityRepository<User, UserEntity> repo;

        public UserRepository(IDesignTimeDbContextFactory<Context> contextFactory)
        {
            EntityFactory<UserEntity> factory = CreateEntityFactory();
            Func<Context, DbSet<UserEntity>> dbSet = CreateFunctionThatReturnsEntityDBSetFromContext();
            repo = new EntityRepository<User, UserEntity>(factory, dbSet, contextFactory);
        }

        private EntityFactory<UserEntity> CreateEntityFactory() => new EntityFactory<UserEntity>(() => new UserEntity());

        private Func<Context, DbSet<UserEntity>> CreateFunctionThatReturnsEntityDBSetFromContext() => c => c.Users;

        public void Add(User model) => repo.Add(model);

        public void Delete(User model) => repo.Delete(model);

        public User Get(object id) => repo.Get(id);

        public IEnumerable<User> GetAll() => repo.GetAll();

        public void Update(User model) => repo.Update(model);
    }
}