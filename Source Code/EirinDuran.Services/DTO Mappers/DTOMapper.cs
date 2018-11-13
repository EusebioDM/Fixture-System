﻿using System;
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
            catch (Exception ex ) when (ex is Domain.DomainException  || ex is ArgumentNullException || ex is IDataAccess.DataAccessException)
            {
                throw new IServices.Exceptions.ServicesException($"DTO had invalid data", ex);
            }
        }

        protected abstract Model TryToMapModel(DTO dto);
    }
}