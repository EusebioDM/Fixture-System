using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;

namespace EirinDuran.Services
{
    public class SportServices
    {
        private readonly ILoginServices loginService;
        private readonly IRepository<Sport> repository;
        private readonly PermissionValidator validator;

        public SportServices(ILoginServices loginService, IRepository<Sport> repository)
        {
            validator = new PermissionValidator(Domain.User.Role.Administrator, loginService);
            this.loginService = loginService;
            this.repository = repository;
        }

        public void Create(Sport sport)
        {
            validator.ValidatePermissions();
            try
            {
                repository.Add(sport);
            }
            catch (ObjectAlreadyExistsInDataBaseException)
            {
                throw new ObjectAlreadyExistsException(sport);
            }
        }
    }
}
