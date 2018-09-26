using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices.DTOs
{
    public class UserDTO
    {
        public string UserName { get; set; }

        public bool IsAdmin { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Password { get; set; }

        public string Mail { get; set; }

        public List<string> FollowedTeamsNames { get; set; }
    }
}
