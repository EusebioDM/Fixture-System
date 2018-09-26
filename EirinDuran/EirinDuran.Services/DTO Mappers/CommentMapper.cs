using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.Services;
using System.Linq;

namespace EirinDuran.Services.DTO_Mappers
{
    internal class CommentMapper
    {
        private IRepository<User> userRepo;

        public CommentMapper(IRepository<User> userRepo){
            this.userRepo = userRepo;
        }

        public CommentDTO Map(Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                UserName = comment.User.UserName,
                TimeStamp = comment.TimeStamp,
                Message = comment.Message
            };
        }

        public Comment Map(CommentDTO commentDTO)
        {
            return new Comment(user: userRepo.Get(commentDTO.UserName),
                message: commentDTO.Message
            );
        }
    }
}