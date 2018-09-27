using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using System.Collections.Generic;

namespace EirinDuran.Services
{
    public class TeamServices
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
                return teamRepository.Get(new Team(teamName));
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

        public void DeleteTeam(string teamName)
        {
            validator.ValidatePermissions();
            try
            {
                teamRepository.Delete(new Team(teamName));
            }
            catch (ObjectDoesntExistsInDataBaseException)
            {
                throw new TeamTryToDeleteDoesNotExistsException();
            }
        }
    }
}