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
            
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                return context.Encounters.Where(encounterHasTeam).Select(mapEntity).ToList();
            }
        }

        public IEnumerable<Encounter> GetBySport(Sport sport)
        {
            Func<EncounterEntity, bool> encounterHasTeam = e => e.Sport.SportName.Equals(sport.Name);

            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                return context.Encounters.Where(encounterHasTeam).Select(mapEntity).ToList();
            }
        }

        public IEnumerable<Encounter> GetByDate(DateTime startDate, DateTime endDate)
        {
            Func<EncounterEntity, bool> encounterHasTeam = e => e.DateTime >= startDate && e.DateTime <= endDate;

            bool a = new DateTime(3000,10,5) >= startDate && new DateTime(3000, 10, 5) <= endDate;
            using (Context context = contextFactory.CreateDbContext(new string[0]))
            {
                return context.Encounters.Where(encounterHasTeam).Select(mapEntity).ToList();
            }
        }
    }
}
