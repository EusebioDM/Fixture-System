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
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to add team.", e);
            }
        }

        public Team GetTeam(string teamName)
        {
            try
            {
                return teamRepository.Get(teamName);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to recover team.", e);
            }
        }

        public IEnumerable<Team> GetAll()
        {
            try
            {
                return teamRepository.GetAll();
            }
            catch(DataAccessException e)
            {
                throw new ServicesException("Failure to try to get all teams.", e);
            }
        }

        public void DeleteTeam(string id)
        {
            validator.ValidatePermissions();
            try
            {
                teamRepository.Delete(id);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to delete team.", e);
            }
        }
    }
}