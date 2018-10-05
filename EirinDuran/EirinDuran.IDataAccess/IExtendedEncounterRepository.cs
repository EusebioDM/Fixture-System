using EirinDuran.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IDataAccess
{
    public interface IExtendedEncounterRepository : IRepository<Encounter>
    {
        IEnumerable<Encounter> GetByTeam(Team river);
    }
}
