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
        private UserMapper UserMapper;
        private readonly Context context;
        public UserRepository(Context aContext)
        {
            context = aContext;
        }

        public void Add (User user) 
        {
            UserMapper = new UserMapper();
            UserEntity userEntity = UserMapper.Map(user);

            context.UserEntities.Add(userEntity);
            context.SaveChanges();
        }

        public void Delete (User user) 
        {
            
        }

        public User Get (int id) 
        {
            UserMapper = new UserMapper();
            UserEntity userEntity = context.UserEntities.Find(id);

            return UserMapper.Map(userEntity);
        }

        public IEnumerable<User> GetAll () 
        {
            UserMapper = new UserMapper();
            List<User> userList = new List<User>();

            foreach(var user in context.UserEntities.ToList())
            {
                userList.Add(UserMapper.Map(user));
            }

            return userList;
        }

        public void Update (User upToDate) 
        {
            
        }
    }
}