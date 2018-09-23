using EirinDuran.Domain.Fixture;
using EirinDuran.IServices;
using EirinDuran.Services;
using System.Linq;

namespace EirinDuran.Services.DTO_Mappers
{
    internal class CommentMapper
    {
        private UserMapper userMapper;

        public CommentMapper()
        {
            userMapper = new UserMapper();
        }

        public CommentDTO Map(Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                User = userMapper.Map(comment.User),
                TimeStamp = comment.TimeStamp,
                Message = comment.Message
            };
        }

        public Comment Map(CommentDTO commentDTO)
        {
            return new Comment(user: userMapper.Map(commentDTO.User), message: commentDTO.Message);
        }
    }
}