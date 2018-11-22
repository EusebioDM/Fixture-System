using System.Collections.Generic;
using SilverFixture.IServices.DTOs;

namespace SilverFixture.IServices.Services_Interfaces
{
    public interface ISportServices
    {
        SportDTO CreateSport(SportDTO sportDTO);

        void ModifySport(SportDTO sportDTO);

        IEnumerable<SportDTO> GetAllSports();

        SportDTO GetSport(string sportId);

        void DeleteSport(string sportId);
    }
}