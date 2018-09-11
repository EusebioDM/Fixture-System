 using EirinDuran.Entities;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    internal class EntityRepository<Model, Entity> : IRepository<Model> where Entity : class, IEntity<Model>
    {
        private EntityFactory<Entity> factory;
        private Func<Context, DbSet<Entity>> getDBSetFunc;
        private Context context;

        public EntityRepository(EntityFactory<Entity> factory, Func<Context, DbSet<Entity>> getDBSetFunc, Context context)
        {
            this.factory = factory;
            this.getDBSetFunc = getDBSetFunc;
            this.context = context;
        }

        public void Add(Model model)
        {
            try
            {
                TryToAdd(model);
            }
            catch (DbUpdateException)
            {
                throw new ObjectAlreadyExistsInDataBaseException();
            }
            catch (SqlException)
            {
                throw new ConnectionToDataAccessFailedException();
            }
        }

        private void TryToAdd(Model model)
        {
            Entity entity = CreateEntity(model);
            Set.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Model model)
        {
            try
            {
                TryToDelete(model);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
            catch (SqlException)
            {
                throw new ConnectionToDataAccessFailedException();
            }
        }

        private void TryToDelete(Model model)
        {
            Entity entity = CreateEntity(model);
            Entity toDelete = Set.Single(e => e.EntityId.Equals(entity.EntityId));
            Set.Remove(toDelete);
            context.SaveChanges();
        }

        public Model Get(string id)
        {
            try
            {
                return TryToGet(id);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
            catch (SqlException)
            {
                throw new ConnectionToDataAccessFailedException();
            }
        }

        private Model TryToGet(string id)
        {
            Entity toReturn = Set.Single(e => e.EntityId.Equals(id));
            return toReturn.ToModel();
        }

        public IEnumerable<Model> GetAll()
        {
            Func<Entity, Model> mapEntity = t => { return t.ToModel(); };
            return Set.Select(mapEntity);
        }

        public void Update(Model model)
        {
            try
            {
                TryToUpdate(model);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
            catch (SqlException)
            {
                throw new ConnectionToDataAccessFailedException();
            }
        }

        private void TryToUpdate(Model model)
        {
            Entity entity = CreateEntity(model);
            Entity toUpdate = Set.Single(e => e.EntityId.Equals(entity.EntityId));
            toUpdate.UpdateWith(model);
            context.SaveChanges();
        }

        private DbSet<Entity> Set => getDBSetFunc.Invoke(context);

        private Entity CreateEntity(Model model)
        {
            Entity entity = factory.CreateEmptyEntity();
            entity.UpdateWith(model);
            return entity;
        }
    }
}
