using EirinDuran.Domain.User;

namespace EirinDuran.WebApi.Models
{
    public class UserModelOut
    {
        public string UserName { get; set; }
      
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }

        public Role Role { get; set; }
    }
}