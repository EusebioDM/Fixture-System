using EirinDuran.Domain.Fixture;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SilverFixture.IDataAccess;
using Microsoft.EntityFrameworkCore;
using EirinDuran.DataAccess;
using EirinDuran.DataAccess.Entities;

namespace EirinDuran.DataAccess
{
    public class ExtendedEncounterRepository : EncounterRepository, IExtendedEncounterRepository
    {
        private IDesignTimeDbContextFactory<Context> contextFactory;
        private Func<EncounterEntity, Encounter> mapEntity = t => { return t.ToModel(); };

        public ExtendedEncounterRepository(IDesignTimeDbContextFactory<Context> contextFactory) : base(contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IEnumerable<Encounter> GetByTeam(string id)
        {
            Func<EncounterEntity, bool> encounterHasTeam = e => e.Teams.Any(t => TeamHasId(t.Team, id));

            return GetFilteredEncounters(encounterHasTeam);
        }

        private bool TeamHasId(TeamEntity team, string id)
        {
            string teamId = team.Name + "_" + team.SportName;
            return teamId.Equals(id, StringComparison.OrdinalIgnoreCase);
        }

        public IEnumerable<Encounter> GetBySport(string sportId)
        {
            Func<EncounterEntity, bool> encounterHasSport = e => e.Sport.SportName.Equals(sportId);

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
