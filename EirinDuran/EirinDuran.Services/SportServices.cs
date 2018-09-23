using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.Services;
using EirinDuran.Services.DTO_Mappers;

namespace EirinDuran.Services
{
    public class SportServices : ISportServices
    {
        private readonly ILoginServices loginService;
        private readonly IRepository<Sport> repository;
        private readonly PermissionValidator validator;
        private readonly SportMapper mapper;

        public SportServices(ILoginServices loginService, IRepository<Sport> repository)
        {
            validator = new PermissionValidator(Domain.User.Role.Administrator, loginService);
            this.loginService = loginService;
            this.repository = repository;
            mapper = new SportMapper();
        }

        public void Create(SportDTO sportDTO)
        {
            validator.ValidatePermissions();
            Sport sport = mapper.Map(sportDTO);
            try
            {
                repository.Add(sport);
            }
            catch (ObjectAlreadyExistsInDataBaseException)
            {
                throw new ObjectAlreadyExistsException(sport);
            }
        }

        public void Modify(SportDTO sportDTO)
        {
            validator.ValidatePermissions();
            Sport sport = mapper.Map(sportDTO);
            repository.Update(sport);
        }
    }
}
