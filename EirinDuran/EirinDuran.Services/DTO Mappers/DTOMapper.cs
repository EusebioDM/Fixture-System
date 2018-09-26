using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Services.DTO_Mappers
{
    internal abstract class DTOMapper<Model, DTO>
    {
        public abstract DTO Map(Model model);

        public Model Map(DTO dto)
        {
            try
            {
                return TryToMapModel(dto);
            }
            catch (Domain.DomainException ex)
            {
                throw new IServices.Exceptions.InvalidaDataException(dto, ex);
            }
        }

        protected abstract Model TryToMapModel(DTO dto);
    }
}
