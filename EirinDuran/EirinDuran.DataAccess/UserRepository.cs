using System;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.Domain;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.Entities;
using EirinDuran.Entities.Mappers;

namespace EirinDuran.DataAccess {
    public class UserRepository : IRepository<User> 
    {
        private readonly Context context;
        public UserRepository(Context aContext)
        {
            context = aContext;
        }

        public void Add (User user) 
        {
            UserEntity userEntity = new UserEntity(user);

            context.Users.Add(userEntity);
            context.SaveChanges();
        }

        public void Delete (User user) 
        {
            
        }

        public User Get (int id) 
        {
            UserEntity userEntity = context.Users.Find(id);

            return userEntity.ToModel();
        }

        public IEnumerable<User> GetAll () 
        {
            List<User> userList = new List<User>();

            foreach(var user in context.Users.ToList())
            {
                userList.Add(user.ToModel());
            }

            return userList;
        }

        public void Update (User upToDate) 
        {
            
        }
    }
}