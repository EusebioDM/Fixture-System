using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;
using EirinDuran.Services.DTO_Mappers;

namespace EirinDuran.Services
{
    public class SportServices : ISportServices
    {
        private readonly IRepository<Sport> sportRepo;
        private readonly IRepository<Team> teamRepo;
        private readonly PermissionValidator validator;
        private readonly SportMapper mapper;

        public SportServices(IRepository<Sport> sportRepo, IRepository<Team> teamRepo)
        {
            validator = new PermissionValidator(Domain.User.Role.Administrator);
            this.sportRepo = sportRepo;
            this.teamRepo = teamRepo;
            mapper = new SportMapper(teamRepo);
        }

        public ILoginServices Login { get; set; }

        public void Create(SportDTO sportDTO)
        {
            validator.ValidatePermissions(Login);
            Sport sport = mapper.Map(sportDTO);
            try
            {
                sportRepo.Add(sport);
            }
            catch (ObjectAlreadyExistsInDataBaseException)
            {
                throw new ObjectAlreadyExistsException(sport);
            }
        }

        public void Modify(SportDTO sportDTO)
        {
            validator.ValidatePermissions(Login);
            Sport sport = mapper.Map(sportDTO);
            sportRepo.Update(sport);
        }
    }
}
