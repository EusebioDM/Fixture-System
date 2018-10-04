using EirinDuran.Domain.Fixture;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EirinDuran.DataAccess
{
    public class ExtendedEncounterRepository : EncounterRepository
    {
        IDesignTimeDbContextFactory<Context> contextFactory;

        public ExtendedEncounterRepository(IDesignTimeDbContextFactory<Context> contextFactory) : base(contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IEnumerable<Encounter> GetByTeam(Team river)
        {
            using(Context context = contextFactory.CreateDbContext(new string[0]))
            {
                IEnumerable<Entities.EncounterEntity> entities = context.Encounters.Where(e => e.AwayTeam.Equals(river) || e.HomeTeam.Equals(river));
                return entities.Select(e => e.ToModel());
            }
        }
    }
}
