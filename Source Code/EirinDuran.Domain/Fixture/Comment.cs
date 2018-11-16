using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.Domain.User;

namespace EirinDuran.Domain.Fixture
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public User.User User { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public string Message { get; private set; }

        public Comment(User.User user, string message)
        {
            Id = Guid.NewGuid();
            User = user;
            Message = message;
            TimeStamp = DateTime.Now;
        }

        public Comment(User.User user, DateTime timeStamp, string message, Guid id)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            User = user;
            TimeStamp = timeStamp == null ? DateTime.Now : timeStamp;
            Message = message;
        }

        public override bool Equals(object obj)
        {
            var comment = obj as Comment;
            return comment != null &&
                   Id.Equals(comment.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
