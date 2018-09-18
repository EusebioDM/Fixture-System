using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using System;
using System.Collections.Generic;

namespace EirinDuran.Services
{
    public class EncounterServices
    {
        private LoginServices loginServices;
        private EncounterRepository encounterRepository;
        private PermissionValidator adminValidator;

        public EncounterServices(EncounterRepository encounterRepository, LoginServices loginServices)
        {
            this.loginServices = loginServices;
            this.encounterRepository = encounterRepository;
            adminValidator = new PermissionValidator(Role.Administrator);
        }

        public void CreateEncounter(Encounter encounter)
        {
            adminValidator.ValidatePermissions(loginServices.LoggedUser);
            encounterRepository.Add(encounter);
        }
    }
}