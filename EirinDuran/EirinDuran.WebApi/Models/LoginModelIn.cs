using System.ComponentModel.DataAnnotations;

namespace EirinDuran.WebApi.Models
{
    public class LoginModelIn
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}