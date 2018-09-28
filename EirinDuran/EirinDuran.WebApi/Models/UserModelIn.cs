using EirinDuran.Domain.User;
using System.ComponentModel.DataAnnotations;

namespace EirinDuran.WebApi.Models
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
    }
}