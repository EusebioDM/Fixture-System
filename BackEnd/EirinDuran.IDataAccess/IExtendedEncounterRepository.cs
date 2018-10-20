using EirinDuran.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IDataAccess
{
    public interface IExtendedEncounterRepository : IRepository<Encounter>
    {
        IEnumerable<Encounter> GetByTeam(string sportId_TeamName);

        IEnumerable<Encounter> GetBySport(string sportId);

        IEnumerable<Encounter> GetByDate(DateTime startDate, DateTime endDate);
    }
}
