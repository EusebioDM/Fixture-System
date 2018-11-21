using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using SilverFixture.IDataAccess;
using SilverFixture.IServices;
using SilverFixture.IServices.DTOs;
using SilverFixture.Services;
using System.Linq;

namespace SilverFixture.Services.DTO_Mappers
{
    internal class CommentMapper : DTOMapper<Comment, CommentDTO>
    {
        private IRepository<User> userRepo;

        public CommentMapper(IRepository<User> userRepo){
            this.userRepo = userRepo;
        }

        public override CommentDTO Map(Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                UserName = comment.User.UserName,
                TimeStamp = comment.TimeStamp,
                Message = comment.Message
            };
        }

        protected override Comment TryToMapModel(CommentDTO commentDTO)
        {
            return new Comment(user: userRepo.Get(commentDTO.UserName),
                message: commentDTO.Message
            );
        }
    }
}