using EirinDuran.Domain.Fixture;
using EirinDuran.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Services.DTO_Mappers
{
    internal class EncounterMapper
    {
        public EncounterDTO Map(Encounter encounter)
        {
            return new EncounterDTO()
            {
                Id = encounter.Id,
                DateTime = encounter.DateTime,
                Comments = encounter.Comments.Select(encounterComment => new CommentDTO()
                {
                    Id = encounterComment.Id,
                    User = new UserDTO()
                    {
                        UserName = encounterComment.User.UserName,
                        Name = encounterComment.User.Name,
                        Surname = encounterComment.User.Surname,
                        Password = encounterComment.User.Password,
                        Mail = encounterComment.User.Mail,
                        FollowedTeams = encounterComment.User.FollowedTeams.Select(encounterCommentUserFollowedTeam => new TeamDTO()
                        {
                            Name = encounterCommentUserFollowedTeam.Name,
                            Logo = encounterCommentUserFollowedTeam.Logo
                        }).ToList()
                    },
                    TimeStamp = encounterComment.TimeStamp,
                    Message = encounterComment.Message
                }).ToList(),
                Sport = new SportDTO()
                {
                    Name = encounter.Sport.Name,
                    Teams = encounter.Sport.Teams.Select(encounterSportTeam => new TeamDTO()
                    {
                        Name = encounterSportTeam.Name,
                        Logo = encounterSportTeam.Logo
                    }).ToList()
                }
            };
        }

        public Encounter Map(EncounterDTO encounterDTO)
        {
            return new Encounter(id: encounterDTO.Id, 
                sport: new Sport(name: encounterDTO.Sport.Name, 
                teams: encounterDTO.Sport.Teams.Select(encounterDTOSportTeam => new Team(name: encounterDTOSportTeam.Name, logo: encounterDTOSportTeam.Logo)).ToList()), 
                teams: null, 
                dateTime: encounterDTO.DateTime, 
                comments: encounterDTO.Comments.Select(encounterDTOComment => new Comment(user: new Domain.User.User(role: default(Domain.User.Role), 
                userName: encounterDTOComment.User.UserName, name: encounterDTOComment.User.Name, surname: encounterDTOComment.User.Surname, 
                password: encounterDTOComment.User.Password, 
                mail: encounterDTOComment.User.Mail, 
                followedTeams: encounterDTOComment.User.FollowedTeams.Select(encounterDTOCommentUserFollowedTeam => new Team(name: encounterDTOCommentUserFollowedTeam.Name, logo: encounterDTOCommentUserFollowedTeam.Logo)).ToList()), 
                timeStamp: encounterDTOComment.TimeStamp, message: encounterDTOComment.Message, id: encounterDTOComment.Id)).ToList());
        }
    }
}
