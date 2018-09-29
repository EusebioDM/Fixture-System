using EirinDuran.Entities;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace EirinDuran.DataAccess
{
    internal class EntityRepository<Model, Entity> where Entity : class, IEntity<Model>
    {
        private EntityFactory<Entity> factory;
        private Func<Context, DbSet<Entity>> getDBSetFunc;
        private IDesignTimeDbContextFactory<Context> contextFactory;
        private EntityUpdater<Entity> entityUpdater;

        public EntityRepository(EntityFactory<Entity> factory, Func<Context, DbSet<Entity>> getDBSetFunc, IDesignTimeDbContextFactory<Context> contextFactory)
        {
            this.factory = factory;
            this.getDBSetFunc = getDBSetFunc;
            this.contextFactory = contextFactory;
            entityUpdater = new EntityUpdater<Entity>(contextFactory);
        }

        public void Add(Model id)
        {
            try
            {
                TryToAdd(id);
            }
            catch (ArgumentException)
            {
                throw new ObjectAlreadyExistsInDataBaseException(id);
            }
            catch (SqlException ex)
            {
                throw new ConnectionToDataAccessFailedException(ex);
            }
        }

        private void TryToAdd(Model id)
        {
            Entity entity = CreateEntity(id);
            ValidateEntityDoesntExistInDataBase(entity);
            entityUpdater.UpdateGraph(entity);
        }

        private void ValidateEntityDoesntExistInDataBase(Entity entity)
        {
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                Entity fromRepo = GetEntityFromRepo(context, entity);
                if (fromRepo != null)
                {
                    throw new ObjectAlreadyExistsInDataBaseException(entity);
                }
            }
        }

        public void Delete(object id)
        {
            try
            {
                TryToDelete(id);
            }
            catch (ArgumentException)
            {
                throw new ObjectDoesntExistsInDataBaseException(id);
            }
            catch (SqlException ex)
            {
                throw new ConnectionToDataAccessFailedException(ex);
            }
        }

        private void TryToDelete(object id)
        {
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                Entity toDelete = context.Find<Entity>(id);
                if(toDelete == null)
                    throw new ObjectDoesntExistsInDataBaseException(id);
                context.Entry(toDelete).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public Model Get(object id)
        {
            try
            {
                return TryToGet(id);
            }
            catch (ArgumentException)
            {
                throw new ObjectDoesntExistsInDataBaseException(id);
            }
            catch (SqlException ex)
            {
                throw new ConnectionToDataAccessFailedException(ex);
            }
        }

        private Model TryToGet(object id)
        {
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                Entity toReturn = context.Find<Entity>(id);
                if (toReturn == null)
                {
                    throw new ObjectDoesntExistsInDataBaseException(id);
                }
                return toReturn.ToModel();
            }
        }

        public IEnumerable<Model> GetAll()
        {
            try
            {
                return TryToGetAll();
            }
            catch (SqlException ex)
            {
                throw new ConnectionToDataAccessFailedException(ex);
            }
        }

        private IEnumerable<Model> TryToGetAll()
        {
            Func<Entity, Model> mapEntity = t => { return t.ToModel(); };
            Entity entity = factory.CreateEmptyEntity();
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                return Set(context).Select(mapEntity).ToList();
            }
        }

        public void Update(Model id)
        {
            try
            {
                TryToUpdate(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ObjectDoesntExistsInDataBaseException(id);
            }
            catch (SqlException ex)
            {
                throw new ConnectionToDataAccessFailedException(ex);
            }
        }

        private void TryToUpdate(Model id)
        {
            Entity entity = CreateEntity(id);
            entityUpdater.UpdateGraph(entity);
        }

        private Entity CreateEntity(Model id)
        {
            Entity entity = factory.CreateEmptyEntity();
            entity.UpdateWith(id);
            return entity;
        }

        private Entity GetEntityFromRepo(Context context, Entity localEntity)
        {
            EntityEntry entry = context.Entry(localEntity);
            object key = HelperFunctions<Entity>.GetKey(entry);
            return context.Find<Entity>(key);
        }

        private DbSet<Entity> Set(Context context) => getDBSetFunc.Invoke(context);

    }
}
