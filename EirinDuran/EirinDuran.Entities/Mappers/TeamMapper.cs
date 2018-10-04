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
                Sport = new SportEntity(team.Sport)
            };
        }

        public Team Map(TeamEntity entity)
        {
            return new Team(
                name: entity.Name,
                sport: entity.Sport.ToModel() ,
                logo: ByteArrayToImage(entity.Logo)
            );
        }

        public void Update(Team source, TeamEntity desination)
        {
            desination.Name = source.Name;
            desination.Logo = ImageToByteArray(source.Logo);
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
