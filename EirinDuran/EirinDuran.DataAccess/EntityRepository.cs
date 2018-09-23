using EirinDuran.Entities;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EirinDuran.DataAccess
{
    internal class EntityRepository<Model, Entity> : IRepository<Model> where Entity : class, IEntity<Model>
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

        public void Add(Model model)
        {
            try
            {
                TryToAdd(model);
            }
            catch (ArgumentException)
            {
                throw new ObjectAlreadyExistsInDataBaseException(model);
            }
            catch (SqlException ex)
            {
                throw new ConnectionToDataAccessFailedException(ex);
            }
        }

        private void TryToAdd(Model model)
        {
            Entity entity = CreateEntity(model);
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

        public void Delete(Model model)
        {
            try
            {
                TryToDelete(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ObjectDoesntExistsInDataBaseException(model);
            }
            catch (SqlException ex)
            {
                throw new ConnectionToDataAccessFailedException(ex);
            }
        }

        private void TryToDelete(Model model)
        {
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                Entity toDelete = CreateEntity(model);
                context.Entry(toDelete).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public Model Get(Model model)
        {
            try
            {
                return TryToGet(model);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException(model);
            }
            catch (SqlException ex)
            {
                throw new ConnectionToDataAccessFailedException(ex);
            }
        }

        private Model TryToGet(Model model)
        {
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                Entity modelTranslation = CreateEntity(model);
                Entity toReturn = GetEntityFromRepo(context, modelTranslation);
                if (toReturn == null)
                {
                    throw new ObjectDoesntExistsInDataBaseException(model);
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

        public void Update(Model model)
        {
            try
            {
                TryToUpdate(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ObjectDoesntExistsInDataBaseException(model);
            }
            catch (SqlException ex)
            {
                throw new ConnectionToDataAccessFailedException(ex);
            }
        }

        private void TryToUpdate(Model model)
        {
            Entity entity = CreateEntity(model);
            entityUpdater.UpdateGraph(entity);
        }

        private Entity CreateEntity(Model model)
        {
            Entity entity = factory.CreateEmptyEntity();
            entity.UpdateWith(model);
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
