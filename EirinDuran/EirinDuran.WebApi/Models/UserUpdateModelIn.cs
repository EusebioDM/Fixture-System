using EirinDuran.Domain.User;
using System.ComponentModel.DataAnnotations;

namespace EirinDuran.WebApi.Models
{
    public class UserUpdateModelIn
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}