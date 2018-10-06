using EirinDuran.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace EirinDuran.Entities.Mappers
{
    internal class TeamMapper
    {

        public TeamEntity Map(Team team)
        {
            return new TeamEntity()
            {
                Name = team.Name,
                Logo = ImageToByteArray(team.Logo),
                Sport = new SportEntity(team.Sport),
                SportName = team.Sport.Name
            };
        }

        public Team Map(TeamEntity entity)
        {
            Team team = new Team(
                name: entity.Name,
                sport: entity.Sport.ToModel()
            );
            if (entity.Logo != null)
            {
                team.Logo = ByteArrayToImage(entity.Logo);
            }
            return team;
        }

        public void Update(Team source, TeamEntity desination)
        {
            desination.Name = source.Name;
            desination.Logo = ImageToByteArray(source.Logo);
            desination.Sport = new SportEntity(source.Sport);
            desination.SportName = source.Sport.Name;
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
