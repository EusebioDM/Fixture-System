﻿using EirinDuran.Domain.Fixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Entities.Mappers
{
    internal class CommentMapper
    {
        public CommentEntity Map(Comment comment)
        {
            return new CommentEntity()
            {
                User = new UserEntity(comment.User),
                TimeStamp = comment.TimeStamp,
                Message = comment.Message
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
        }
    }
}