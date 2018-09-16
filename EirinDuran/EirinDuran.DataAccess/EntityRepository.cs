﻿using EirinDuran.Entities;
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
            foreach (NavigationEntry property in entry.Navigations)
            {
                // The current value of a propety is returned as an object by EF so casting is needed to determine whether 
                // the propety is a single navigatable propety or a collection on navigatables
                try
                {   
                    List<object> entries = TryToCastPropetyToCollectionOfEntries(context, property);
                    UpdateEntityCollection(context, entries);
                }
                catch (ArgumentException) // Casting failed since property is a single navigatable propety
                {
                    UpdateSingleEntry(context, property);
                }
            }
            try
            {
                context.Update(entry.Entity);
            }
            catch (InvalidOperationException) // Update failed since another instance of this entity is already present in context
            {
                if (!entry.IsKeySet)
                {
                    entry.State = EntityState.Unchanged;
                }
            }
        }

        private List<object> TryToCastPropetyToCollectionOfEntries(Context context, NavigationEntry property)
        {
            List<object> entries = (property.CurrentValue as IEnumerable<object>).Cast<object>().ToList();
            return entries;
        }

        private void UpdateEntityCollection(Context context, List<object> entries)
        {
            foreach (object obj in entries)
            {
                EntityEntry childEntry = context.Entry(obj);
                UpdateEntityRec(context, childEntry);
            }
        }

        private void UpdateSingleEntry(Context context, NavigationEntry property)
        {
            EntityEntry childEntry = context.Entry(property.CurrentValue);
            UpdateEntityRec(context, childEntry);
        }
    }
}
