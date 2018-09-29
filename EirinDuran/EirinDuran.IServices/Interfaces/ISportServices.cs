
using EirinDuran.IServices.DTOs;
using System.Collections.Generic;

namespace EirinDuran.IServices.Interfaces
{
    public interface ISportServices
    {
        void Create(SportDTO sportDTO);

        void Modify(SportDTO sportDTO);

        IEnumerable<SportDTO> GetAllSports();
    }
}