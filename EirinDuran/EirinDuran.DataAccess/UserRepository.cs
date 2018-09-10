using System;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.Domain;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.Entities;

namespace EirinDuran.DataAccess {
    public class UserRepository : IRepository<User> 
    {
        private EntityMapper entityMapper;
        private readonly Context context;
        public UserRepository(Context aContext)
        {
            context = aContext;
        }

        public void Add (User user) 
        {
            entityMapper = new EntityMapper();
            UserEntity userEntity = entityMapper.Map(user);

            context.UserEntities.Add(userEntity);
            context.SaveChanges();
        }

        public void Delete (User user) 
        {
            
        }

        public User Get (int id) 
        {
            entityMapper = new EntityMapper();
            UserEntity userEntity = context.UserEntities.Find(id);

            return entityMapper.Map(userEntity);
        }

        public IEnumerable<User> GetAll () 
        {
            entityMapper = new EntityMapper();
            List<User> userList = new List<User>();

            foreach(var user in context.UserEntities.ToList())
            {
                userList.Add(entityMapper.Map(user));
            }

            return userList;
        }

        public void Update (User upToDate) 
        {
            
        }
    }
}