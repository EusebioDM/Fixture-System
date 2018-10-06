using EirinDuran.Domain.User;
using EirinDuran.IServices.DTOs;
using System.ComponentModel.DataAnnotations;

namespace EirinDuran.WebApi.Models
{
    public class UserUpdateModelIn
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public UserUpdateModelIn() { }

        public UserDTO Map()
        {
            return new UserDTO()
            {
                IsAdmin = IsAdmin,
                Name = Name,
                Surname = Surname,
                Password = Password,
                Mail = Mail
            };
        }
    }
}