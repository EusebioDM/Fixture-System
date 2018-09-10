using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    public class EncounterRepository : IRepository<Encounter>
    {
        public void Add(Encounter encounter)
        {
            try
            {
                TryToAdd(encounter);
            }
            catch (DbUpdateException)
            {
                throw new ObjectAlreadyExistsInDataBaseException();
            }
        }

        private void TryToAdd(Encounter encounter)
        {
            using (Context context = new Context())
            {
                context.Encounters.Add(new EncounterEntity(encounter));
                context.SaveChanges();
            }
        }

        public void Delete(Encounter encounter)
        {
            try
            {
                TryToDelete(encounter);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
        }

        private void TryToDelete(Encounter encounter)
        {
            using (Context context = new Context())
            {
                EncounterEntity toDelete = context.Encounters.Single(e => e.Id.Equals(encounter.Id));
                context.Encounters.Remove(toDelete);
                context.SaveChanges();
            }
        }

        public Encounter Get(int id)
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

        private Encounter TryToGet(int id)
        {
            using (Context context = new Context())
            {
                EncounterEntity toReturn = context.Encounters.Single(e => e.Id.Equals(id));
                return toReturn.ToModel();
            }
        }

        public IEnumerable<Encounter> GetAll()
        {
            using (Context context = new Context())
            {
                Func<EncounterEntity, Encounter> mapEntity = e => { return e.ToModel(); };
                return context.Encounters.Select(mapEntity);
            }
        }

        public void Update(Encounter encounter)
        {
            try
            {
                TryToUpdate(encounter);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
        }

        private void TryToUpdate(Encounter encounter)
        {
            using (Context context = new Context())
            {
                EncounterEntity toUpdate = context.Encounters.Single(e => e.Id.Equals(encounter.Id));
                toUpdate.UpdateWith(encounter);
                context.SaveChanges();
            }
        }
    }
}
