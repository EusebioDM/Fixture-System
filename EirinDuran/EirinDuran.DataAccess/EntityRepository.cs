using EirinDuran.Entities;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        private Func<IContext, Microsoft.EntityFrameworkCore.DbSet<Entity>> getDBSetFunc;
        private IContextFactory contextFactory;
        private EntityUpdater<Entity> entityUpdater;

        public EntityRepository(EntityFactory<Entity> factory, Func<IContext, Microsoft.EntityFrameworkCore.DbSet<Entity>> getDBSetFunc, IContextFactory contextFactory)
        {
            this.factory = factory;
            this.getDBSetFunc = getDBSetFunc;
            this.contextFactory = contextFactory;
            entityUpdater = new EntityUpdater<Entity>();
        }

        public void Add(Model model)
        {
            try
            {
                TryToAdd(model);
            }
            catch (ArgumentException ex)
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
            using (Context context = contextFactory.GetNewContext())
            {
                ValidateAlternateKey(context, entity);
                entityUpdater.UpdateEntityWithItsChildren(context, entity);
                context.SaveChanges();
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
                throw new ObjectDoesntExistsInDataBaseException();
            }
            catch (SqlException)
            {
                throw new ConnectionToDataAccessFailedException();
            }
        }

        private void TryToDelete(Model model)
        {
            using (Context context = contextFactory.GetNewContext())
            {
                Entity toDelete = CreateEntity(model);
                Set(context).Remove(toDelete);
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
                throw new ObjectDoesntExistsInDataBaseException();
            }
            catch (SqlException)
            {
                throw new ConnectionToDataAccessFailedException();
            }
        }

        private Model TryToGet(Model model)
        {
            Model toReturn;
            using (Context context = contextFactory.GetNewContext())
            {
                Entity entity = CreateEntity(model);
                toReturn = Set(context).Single(e => e.GetAlternateKey().Equals(entity.GetAlternateKey())).ToModel();
            }
            return toReturn;
        }

        public IEnumerable<Model> GetAll()
        {
            Func<Entity, Model> mapEntity = t => { return t.ToModel(); };
            Entity entity = factory.CreateEmptyEntity();
            List<Model> models;
            using (Context context = contextFactory.GetNewContext())
            {
                models = Set(context).Select(mapEntity).ToList();
            }
            return models;
        }

        public void Update(Model model)
        {
            try
            {
                TryToUpdate(model);
            }
            catch (DbUpdateConcurrencyException)
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
            using (Context context = contextFactory.GetNewContext())
            {
                entityUpdater.UpdateEntityWithItsChildren(context, entity);
                context.SaveChanges();
            }
        }

        private Microsoft.EntityFrameworkCore.DbSet<Entity> Set(Context context) => getDBSetFunc.Invoke(context);

        private Entity CreateEntity(Model model)
        {
            Entity entity = factory.CreateEmptyEntity();
            entity.UpdateWith(model);
            AssignIdIfMissing(entity);
            return entity;
        }

        private void AssignIdIfMissing(Entity entity)
        {
            using (Context context = contextFactory.GetNewContext())
            {
                Entity fromDb = Set(context).FirstOrDefault(e => entity.GetAlternateKey().Equals(e.GetAlternateKey()));
                if (fromDb != null)
                {
                    entity.Id = fromDb.Id;
                }
                else
                {
                    ValidateAlternateKey(context, entity);
                }
            }
        }

        private void ValidateAlternateKey(Context context, Entity entity)
        {
            bool alternateKeyAlreadyInDB = Set(context).Any(e => e.GetAlternateKey().Equals(entity.GetAlternateKey()));

            if (alternateKeyAlreadyInDB)
            {
                throw new ObjectAlreadyExistsInDataBaseException();
            }
        }
    }
}
