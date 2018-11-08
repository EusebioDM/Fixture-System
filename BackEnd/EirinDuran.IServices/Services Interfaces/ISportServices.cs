using System.Collections.Generic;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Services_Interfaces
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