using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.Domain.User;

namespace EirinDuran.Domain.Fixture
{
    public class Comment
    {
        public User.User User { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public string Message { get; private set; }

        public Comment(User.User user, string message)
        {
            User = user;
            Message = message;
            TimeStamp = DateTime.Now;
        }

        public Comment(User.User user, DateTime timeStamp, string message)
        {
            User = user;
            TimeStamp = timeStamp;
            Message = message;
        }
    }
}
