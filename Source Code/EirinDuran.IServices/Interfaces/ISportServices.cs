
using EirinDuran.IServices.DTOs;
using System.Collections.Generic;

namespace EirinDuran.IServices.Interfaces
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