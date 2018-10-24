using System;

namespace EirinDuran.DataAccess.Entities
{
    public class EncounterTeam
    {
        public Guid EncounterId { get; set; }
        public virtual EncounterEntity Encounter { get; set; }
        public string TeamName { get; set; }
        public string SportName { get; set; }
        public string TeamNameFk { get; set; }
        public string SportNameFk { get; set; }
        public virtual TeamEntity Team { get; set; }

        public EncounterTeam()
        {
        }

        public EncounterTeam(EncounterEntity encounter, TeamEntity team)
        {
            Encounter = encounter;
            EncounterId = Encounter.Id;
            Team = team;
            TeamName = Team.Name;
            SportName = Team.SportName;
            TeamNameFk = TeamName;
            SportNameFk = SportName;
        }
    }
}