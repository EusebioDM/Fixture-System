using SilverFixture.IServices.DTOs;
using System;

namespace EirinDuran.WebApi.Models
{
    public class UserModelOut
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }

        public bool IsAdmin { get; set; }

        public string CommentariesUrl { get; }

        public UserModelOut(UserDTO user)
        {
            UserName = user.UserName;
            Name = user.Name;
            Surname = user.Surname;
            Mail = user.Mail;
            IsAdmin = user.IsAdmin;
            CommentariesUrl = "/api/users/" + UserName + "/commentaries";
        }

        public override bool Equals(object obj)
        {
            var @out = obj as UserModelOut;
            return @out != null &&
                   UserName == @out.UserName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserName);
        }
    }
}