using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.DataAccess.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.DataAccess.Entities
{
    public class CommentEntity : IEntity<Comment>
    {
        public virtual UserEntity User { get; set; }
        public DateTime TimeStamp { get;  set; }
        public string Message { get;  set; }
        public Guid Id { get; set; }
        private CommentMapper mapper;

        public CommentEntity()
        {
            mapper = new CommentMapper();
        }

        public CommentEntity(Comment model) : this()
        {
            UpdateWith(model);
        }

        public Comment ToModel()
        {
            return mapper.Map(this);
        }

        public void UpdateWith(Comment model)
        {
            mapper.Update(model, this);
        }
    }
}
