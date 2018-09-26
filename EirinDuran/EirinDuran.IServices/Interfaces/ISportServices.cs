
using EirinDuran.IServices.DTOs;

namespace EirinDuran.IServices.Interfaces
{
    public interface ISportServices
    {
        void Create(SportDTO sportDTO);

        void Modify(SportDTO sportDTO);
    }
}