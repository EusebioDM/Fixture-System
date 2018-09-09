using EirinDuran.Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public Role Role { get; set; }
    }
}
