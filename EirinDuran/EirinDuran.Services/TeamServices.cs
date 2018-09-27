using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;

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
    }
}