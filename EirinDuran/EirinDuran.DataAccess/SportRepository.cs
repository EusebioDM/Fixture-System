﻿using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    public class SportRepository : IRepository<Sport>
    {
        public void Add(Sport sport)
        {
            try
            {
                TryToAdd(sport);
            }
            catch (DbUpdateException)
            {
                throw new ObjectAlreadyExistsInDataBaseException();
            }
        }

        private void TryToAdd(Sport sport)
        {
            using (Context context = new Context())
            {
                context.Sports.Add(new SportEntity(sport));
                context.SaveChanges();
            }
        }

        public void Delete(Sport sport)
        {
            try
            {
                TryToDelete(sport);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
        }

        private void TryToDelete(Sport sport)
        {
            using (Context context = new Context())
            {
                SportEntity toDelete = context.Sports.Single(s => s.Equals(sport.Name));
                context.Sports.Remove(toDelete);
                context.SaveChanges();
            }
        }

        public Sport Get(int id)
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

        private Sport TryToGet(int id)
        {
            using (Context context = new Context())
            {
                SportEntity toReturn = context.Sports.Single(t => t.Name.Equals(id));
                return toReturn.ToModel();
            }
        }

        public IEnumerable<Sport> GetAll()
        {
            using (Context context = new Context())
            {
                Func<SportEntity, Sport> mapEntity = s => { return s.ToModel(); };
                return context.Sports.Select(mapEntity);
            }
        }

        public void Update(Sport sport)
        {
            try
            {
                TryToUpdate(sport);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
        }

        private void TryToUpdate(Sport sport)
        {
            using (Context context = new Context())
            {
                SportEntity toUpdate = context.Sports.Single(s => s.Name.Equals(sport.Name));
                toUpdate.UpdateWith(sport);
                context.SaveChanges();
            }
        }
    }
}
