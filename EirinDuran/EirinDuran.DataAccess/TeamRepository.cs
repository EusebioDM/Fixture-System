using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using EirinDuran.Entities.Mappers;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccess
{
    public class TeamRepository : IRepository<Team>
    {

        public void Add(Team team)
        {
            try
            {
                TryToAdd(team);
            }
            catch (DbUpdateException)
            {
                throw new ObjectAlreadyExistsInDataBaseException();
            }
        }

        private void TryToAdd(Team team)
        {
            using (Context context = new Context())
            {
                context.Teams.Add(new TeamEntity(team));
                context.SaveChanges();
            }
        }

        public void Delete(Team team)
        {
            try
            {
                TryToDelete(team);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
        }

        private void TryToDelete(Team team)
        {
            using (Context context = new Context())
            {
                TeamEntity toDelete = context.Teams.Single(t => t.Equals(team));
                context.Teams.Remove(toDelete);
                context.SaveChanges();
            }
        }

        public Team Get(int id)
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

        private Team TryToGet(int id)
        {
            using (Context context = new Context())
            {
                TeamEntity toReturn = context.Teams.Single(t => t.Name.Equals(id));
                return toReturn.ToModel();
            }
        }

        public IEnumerable<Team> GetAll()
        {
            using (Context context = new Context())
            {
                Func<TeamEntity, Team> mapEntity = t => { return t.ToModel(); };
                return context.Teams.Select(mapEntity);
            }
        }

        public void Update(Team team)
        {
            try
            {
                TryToUpdate(team);
            }
            catch (InvalidOperationException)
            {
                throw new ObjectDoesntExistsInDataBaseException();
            }
        }

        private void TryToUpdate(Team team)
        {
            using (Context context = new Context())
            {
                TeamEntity toUpdate = context.Teams.Single(t => t.Name.Equals(team.Name));
                toUpdate.UpdateWith(team);
                context.SaveChanges();
            }
        }
    }
}
