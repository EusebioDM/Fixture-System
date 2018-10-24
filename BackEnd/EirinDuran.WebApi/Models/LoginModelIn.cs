using EirinDuran.IServices.DTOs;
using System.ComponentModel.DataAnnotations;

namespace EirinDuran.WebApi.Models
{
    public class LoginModelIn
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public LoginModelIn()
        {
        }

        public LoginModelIn(UserDTO user)
        {
            UserName = user.UserName;
            Password = user.Password;
        }

        public UserDTO ToServicesDTO()
        {
            return new UserDTO()
            {
                UserName = UserName,
                Password = Password
            };
        }
    }
}