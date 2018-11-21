using SilverFixture.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilverFixture.DataAccess.Entities.Mappers
{
    internal class CommentMapper
    {
        public CommentEntity Map(Comment comment)
        {
            return new CommentEntity()
            {
                User = new UserEntity(comment.User),
                TimeStamp = comment.TimeStamp,
                Message = comment.Message,
                Id = comment.Id
            };
        }

        public Comment Map(CommentEntity entity)
        {
            return new Comment(entity.User.ToModel(), entity.TimeStamp, entity.Message, entity.Id);
        }

        public void Update(Comment source, CommentEntity destination)
        {
            destination.User = new UserEntity(source.User);
            destination.TimeStamp = source.TimeStamp;
            destination.Message = source.Message;
            destination.Id = source.Id;
        }
    }
}
