using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.Interfaces;
using System.Collections.Generic;

namespace EirinDuran.Services
{
    public class TeamServices : ITeamServices
    {
        private readonly ILoginServices loginServices;
        private readonly IRepository<Team> teamRepository;
        private readonly PermissionValidator validator;

        public TeamServices(ILoginServices loginServices, IRepository<Team> teamRepository)
        {
            this.teamRepository = teamRepository;
            this.loginServices = loginServices;
            validator = new PermissionValidator(Domain.User.Role.Administrator, loginServices);
        }

        public void AddTeam(Team team)
        {
            validator.ValidatePermissions();
            try
            {
                teamRepository.Add(team);
            }
            catch (ObjectAlreadyExistsInDataBaseException)
            {
                throw new TeamTryToAddAlreadyExistsException();
            }
        }

        public Team GetTeam(string teamName)
        {
            try
            {
                return teamRepository.Get(teamName);
            }
            catch (ObjectDoesntExistsInDataBaseException)
            {
                throw new TeamTryToRecoverDoesNotExistException();
            }
        }

        public IEnumerable<Team> GetAll()
        {
            return teamRepository.GetAll();
        }

        public void DeleteTeam(string id)
        {
            validator.ValidatePermissions();
            try
            {
                teamRepository.Delete(id);
            }
            catch (ObjectDoesntExistsInDataBaseException)
            {
                throw new TeamTryToDeleteDoesNotExistsException();
            }
        }
    }
}