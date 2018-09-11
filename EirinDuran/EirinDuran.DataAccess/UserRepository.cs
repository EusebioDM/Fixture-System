using EirinDuran.Domain;
using EirinDuran.Domain.User;
using EirinDuran.Entities;
using EirinDuran.Entities.Mappers;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.DataAccess
{
    public class UserRepository : IRepository<User>
    {
        private IContext context;

        public UserRepository(IContext context)
        {
            this.context = context;
        }

        public void Add(User user)
        {
            try
            {
                TryToAdd(user);
            }
            catch (DbUpdateException)
            {
                throw new ObjectAlreadyExistsInDataBaseException();
            }
        }

        private void TryToAdd(User user)
        {
            context.Users.Add(new UserEntity(user));
            context.SaveChanges();
        }

        public void Delete(User user)
        {
            try
            {
                TryToDelete(user);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
        }

        private void TryToDelete(User user)
        {
            UserEntity toDelete = context.Users.Single(u => u.Name.Equals(user.Name));
            context.Users.Remove(toDelete);
            context.SaveChanges();
        }

        public User Get(int id)
        {
            try
            {
                return TryToGet(id);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
        }

        private User TryToGet(int id)
        {
            UserEntity toReturn = context.Users.Single(t => t.Id.Equals(id));
            return toReturn.ToModel();
        }

        public IEnumerable<User> GetAll()
        {
            Func<UserEntity, User> mapEntity = u => { return u.ToModel(); };
            return context.Users.Select(mapEntity);
        }

        public void Update(User user)
        {
            try
            {
                TryToUpdate(user);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
        }

        private void TryToUpdate(User user)
        {
            UserEntity toUpdate = context.Users.Single(u => u.Name.Equals(user.Name));
            toUpdate.UpdateWith(user);
            context.SaveChanges();
        }
    }
}