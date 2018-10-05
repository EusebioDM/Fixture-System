using EirinDuran.Domain.Fixture;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EirinDuran.Entities;

namespace EirinDuran.DataAccess
{
    public class ExtendedEncounterRepository : EncounterRepository
    {
        private IDesignTimeDbContextFactory<Context> contextFactory;
        private Func<EncounterEntity, Encounter> mapEntity = t => { return t.ToModel(); };

        public ExtendedEncounterRepository(IDesignTimeDbContextFactory<Context> contextFactory) : base(contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IEnumerable<Encounter> GetByTeam(Team river)
        {
            Func<EncounterEntity, bool> encounterHasTeam = e => e.AwayTeam.Name.Equals(river.Name) || e.HomeTeam.Name.Equals(river.Name);

            return GetFilteredEncounters(encounterHasTeam);
        }

        public IEnumerable<Encounter> GetBySport(Sport sport)
        {
            Func<EncounterEntity, bool> encounterHasSport = e => e.Sport.SportName.Equals(sport.Name);

            return GetFilteredEncounters(encounterHasSport);
        }

        public IEnumerable<Encounter> GetByDate(DateTime startDate, DateTime endDate)
        {
            Func<EncounterEntity, bool> encounterIsInBetweenDates = e => e.DateTime >= startDate && e.DateTime <= endDate;

            return GetFilteredEncounters(encounterIsInBetweenDates);
        }

        private IEnumerable<Encounter> GetFilteredEncounters(Func<EncounterEntity, bool> predicate)
        {
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                return context.Encounters.Where(predicate).Select(mapEntity).ToList();
            }
        }
    }
}
