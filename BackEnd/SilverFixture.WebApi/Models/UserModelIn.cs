using SilverFixture.Domain.User;
using SilverFixture.IServices.DTOs;
using System.ComponentModel.DataAnnotations;

namespace SilverFixture.WebApi.Models
{
    public class UserModelIn
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        public UserModelIn()
        {
        }

        public UserModelIn(UserDTO user)
        {
            UserName = user.UserName;
            Name = user.Name;
            Surname = user.Surname;
            Mail = user.Mail;
            Password = user.Password;
            IsAdmin = user.IsAdmin;
        }

        public UserDTO ToServicesDTO( )
        {
            return new UserDTO()
            {
                UserName = UserName,
                IsAdmin = IsAdmin,
                Name = Name,
                Surname = Surname,
                Password = Password,
                Mail = Mail
            };
        }
    }
}