using System;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using SilverFixture.IDataAccess;
using SilverFixture.IServices.DTOs;
using SilverFixture.IServices.Exceptions;
using SilverFixture.IServices.Services_Interfaces;
using SilverFixture.Services.DTO_Mappers;

namespace SilverFixture.Services
{
    public class EncounterQueryServices : IEncounterQueryServices
    {
        private readonly ILoginServices loginServices;
        private readonly IExtendedEncounterRepository encounterRepository;
        private readonly IRepository<Sport> sportRepo;
        private readonly IRepository<Team> teamRepo;
        private readonly IRepository<User> userRepo;
        private readonly IRepository<Comment> commentRepo;
        private readonly EncounterMapper encounterMapper;
        private readonly CommentMapper commentMapper;

        public EncounterQueryServices(ILoginServices loginServices, IExtendedEncounterRepository encounterRepository, IRepository<Sport> sportRepo, IRepository<Team> teamRepo, IRepository<User> userRepo, IRepository<Comment> commentRepo)
        {
            this.loginServices = loginServices;
            this.encounterRepository = encounterRepository;
            this.sportRepo = sportRepo;
            this.teamRepo = teamRepo;
            this.userRepo = userRepo;
            this.commentRepo = commentRepo;
            encounterMapper = new EncounterMapper(sportRepo, teamRepo, commentRepo);
            commentMapper = new CommentMapper(userRepo);
        }

        public IEnumerable<EncounterDTO> GetAllEncountersWithFollowedTeams()
        {
            List<Encounter> encountersWithComment = new List<Encounter>();
            IEnumerable<Encounter> allEncounters = encounterRepository.GetAll();
            foreach (var encounter in allEncounters)
            {
                bool intersect = encounter.Teams.Select(t => t.Name.ToString()).Intersect(loginServices.LoggedUser.FollowedTeamsNames).Any();
                bool hasComments = (encounter.Comments.Any());

                if (intersect && hasComments)
                {
                    encountersWithComment.Add(encounter);
                }
            }

            return encountersWithComment.Select(e => encounterMapper.Map(e));
        }
        
        public IEnumerable<EncounterDTO> GetEncountersBySport(string sportName)
        {
            try
            {
                return encounterRepository.GetBySport(sportName).Select(e => encounterMapper.Map(e));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to recover encounter with {sportName} sport name.", e);
            }
        }

        public IEnumerable<EncounterDTO> GetEncountersByTeam(string teamId)
        {
            try
            {
                return encounterRepository.GetByTeam(teamId).Select(e => encounterMapper.Map(e));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to get all encounters of a team with id {teamId}.", e);
            }
        }

        public IEnumerable<EncounterDTO> GetEncountersByDate(DateTime start, DateTime end)
        {
            IEnumerable<Encounter> encounters = encounterRepository.GetByDate(start, end);
            return encounters.Select(e => encounterMapper.Map(e));
        }

        public IEnumerable<CommentDTO> GetAllCommentsToOneEncounter(string encounterId)
        {
            try
            {
                
                List<CommentDTO> comments = encounterRepository.Get(encounterId).Comments.Select(c => commentMapper.Map(c)).ToList();
                comments.ForEach(c => c.EncounterId = encounterId);

                return comments;
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to recover all commentaries from encounter with id " + encounterId, e);
            }
        }
    }
}