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

        public EntityRepository(EntityFactory<Entity> factory, Func<IContext, Microsoft.EntityFrameworkCore.DbSet<Entity>> getDBSetFunc, IContextFactory contextFactory)
        {
            this.factory = factory;
            this.getDBSetFunc = getDBSetFunc;
            this.contextFactory = contextFactory;
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
                UpdateEntity(context, entity);
                context.SaveChanges();
            }
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
            using (Context context = contextFactory.GetNewContext())
            {
                Entity toDelete = GetLoadedEntity(context, model);
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
            using (Context context = contextFactory.GetNewContext())
            {
                Entity toReturn = GetLoadedEntity(context, model);
                return toReturn.ToModel();
            }
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
            catch (DbUpdateConcurrencyException ex)
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
                UpdateEntity(context, entity);
                context.SaveChanges();
            }
        }

        private Microsoft.EntityFrameworkCore.DbSet<Entity> Set(Context context) => getDBSetFunc.Invoke(context);

        private Entity CreateEntity(Model model)
        {
            Entity entity = factory.CreateEmptyEntity();
            entity.UpdateWith(model);
            AssignIDIfMissing(entity);
            return entity;
        }

        private void AssignIDIfMissing(Entity entity)
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

        private Entity GetLoadedEntity(Context context, Model model)
        {
            Entity entity = CreateEntity(model);
            ICollection<Entity> allEntities = Set(context).ToList();

            if (entity.NavegablePropeties.Equals(""))
            {
                entity = Set(context).Single(e => e.GetAlternateKey().Equals(entity.GetAlternateKey()));
            }
            else
            {
                entity = Set(context).Include(entity.NavegablePropeties).Single(e => e.GetAlternateKey().Equals(entity.GetAlternateKey()));
            }
            return entity;
        }

        private void ValidateAlternateKey(Context context, Entity entity)
        {
            bool alternateKeyAlreadyInDB = Set(context).Any(e => e.GetAlternateKey().Equals(entity.GetAlternateKey()));

            if (alternateKeyAlreadyInDB)
            {
                throw new ObjectAlreadyExistsInDataBaseException();
            }
        }

        private void UpdateEntity(Context context, Entity entity)
        {
            EntityEntry<Entity> entry = context.Entry<Entity>(entity);
            UpdateEntityRec(context, entry);
        }

        private void UpdateEntityRec(Context context, EntityEntry entry)
        {
            foreach (var p in entry.Navigations)
            {
                try
                {
                    List<object> list = (p.CurrentValue as IEnumerable<object>).Cast<object>().ToList();
                    foreach (object obj in list)
                    {
                        EntityEntry childEntry = context.Entry(obj);
                        UpdateEntityRec(context, childEntry);
                    }
                }
                catch (ArgumentException)
                {
                    EntityEntry childEntry = context.Entry(p.CurrentValue);
                    UpdateEntityRec(context, childEntry);
                }
            }
            try
            {
                context.Update(entry.Entity);
            }
            catch (InvalidOperationException)
            {
                if (!entry.IsKeySet)
                {
                    entry.State = EntityState.Unchanged;
                }
            }
        }

    }
}
