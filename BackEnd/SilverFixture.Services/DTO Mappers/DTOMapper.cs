using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.Domain;
using SilverFixture.IServices.Exceptions;

namespace SilverFixture.Services.DTO_Mappers
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
            catch(DomainException ex)
            {
                throw new ServicesException(ex.Message, ex);
            }
            catch (Exception ex ) when (ex is ArgumentNullException || ex is SilverFixture.IDataAccess.DataAccessException)
            {
                throw new ServicesException(ex.Message, ex);
            }
        }

        protected abstract Model TryToMapModel(DTO dto);
    }
}
