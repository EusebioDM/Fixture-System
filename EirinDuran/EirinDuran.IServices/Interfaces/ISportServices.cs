
using EirinDuran.IServices.DTOs;
using System.Collections.Generic;

namespace EirinDuran.IServices.Interfaces
{
    public interface ISportServices
    {
        void CreateSport(SportDTO sportDTO);

        void ModifySport(SportDTO sportDTO);

        IEnumerable<SportDTO> GetAllSports();

        void DeleteSport(string sportId);
    }
}