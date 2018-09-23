using EirinDuran.Domain.Fixture;
using EirinDuran.IServices;
using EirinDuran.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("EirinDuran.ServicesTest")]

namespace EirinDuran.Services.DTO_Mappers
{
    internal class EncounterMapper
    {
        private SportMapper sportMapper;
        private TeamMapper teamMapper;
        private CommentMapper commentMapper;

        public EncounterMapper()
        {
            sportMapper = new SportMapper();
            teamMapper = new TeamMapper();
            commentMapper = new CommentMapper();
        }

        public EncounterDTO Map(Encounter encounter)
        {
            return new EncounterDTO()
            {
                Id = encounter.Id,
                DateTime = encounter.DateTime,
                Comments = encounter.Comments.Select(c => commentMapper.Map(c)).ToList(),
                Sport = sportMapper.Map(encounter.Sport),
                HomeTeam = new TeamDTO() { Name = encounter.Teams.First().Name, Logo = encounter.Teams.First().Logo },
                AwayTeam = new TeamDTO() { Name = encounter.Teams.Last().Name, Logo = encounter.Teams.Last().Logo }
            };
        }

        public Encounter Map(EncounterDTO encounterDTO)
        {
            return new Encounter(id: encounterDTO.Id,
                teams: new List<Team> { teamMapper.Map(encounterDTO.HomeTeam), teamMapper.Map(encounterDTO.AwayTeam) },
                comments: encounterDTO.Comments.Select(c => commentMapper.Map(c)).ToList(),
                dateTime: encounterDTO.DateTime,
                sport: sportMapper.Map(encounterDTO.Sport));
        }
    }
}
