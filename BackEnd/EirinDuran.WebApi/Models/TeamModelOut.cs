using SilverFixture.IServices.DTOs;
using System;

namespace EirinDuran.WebApi.Models
{
    public class TeamModelOut
    {
        public string Name { get; set; }

        public string Logo { get; set; }

        public string SportName { get; set; }

        public string GetTeamEncounters { get; }

        public string FollowTeam{ get; }

        public TeamModelOut(TeamDTO team)
        {
            Name = team.Name;
            SportName = team.SportName;
            Logo = team.Logo;
            GetTeamEncounters = "/api/teams/" + Name + "_" + SportName + "/encounters";
            FollowTeam = "/api/teams/" + Name + "_" + SportName + "/follower";
        }

        public override bool Equals(object obj)
        {
            var @out = obj as TeamDTO;
            return @out != null &&
                   SportName == @out.SportName &&
                   Name == @out.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}