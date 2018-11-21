using SilverFixture.Domain.Fixture;
using SilverFixture.IDataAccess;
using SilverFixture.IServices;
using SilverFixture.IServices.DTOs;
using SilverFixture.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace SilverFixture.Services.DTO_Mappers
{
    internal class TeamMapper : DTOMapper<Team, TeamDTO>
    {
        private IRepository<Sport> repo;

        public TeamMapper(IRepository<Sport> sportRepo)
        {
            repo = sportRepo;
        }
        public override TeamDTO Map(Team team)
        {
            return new TeamDTO()
            {
                Name = team.Name,
                Logo = EncondeImage(team.Logo),
                SportName = team.Sport.Name
            };
        }

        protected override Team TryToMapModel(TeamDTO teamDTO)
        {
            Sport sport = repo.Get(teamDTO.SportName);
            Team team = new Team(name: teamDTO.Name, sport: sport);
            if (teamDTO.Logo != null)
            {
                team.Logo = DecodeImage(teamDTO.Logo);
            }

            return team;
        }

        private string EncondeImage(Image image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imageBytes = stream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }

        private Image DecodeImage(string base64Image)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);
            MemoryStream stream = new MemoryStream(imageBytes);
            return Image.FromStream(stream);
        }
    }
}
