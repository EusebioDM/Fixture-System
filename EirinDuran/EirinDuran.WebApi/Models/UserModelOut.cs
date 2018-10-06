
using EirinDuran.IServices.DTOs;

namespace EirinDuran.WebApi.Models
{
    public class UserModelOut
    {
        public string UserName { get; set; }
      
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }

        public bool IsAdmin { get; set; }

        public UserModelOut(UserDTO user)
        {
            UserName = user.UserName;
            Name = user.Name;
            Surname = user.Surname;
            Mail = user.Mail;
            IsAdmin = user.IsAdmin;
        }
    }
}