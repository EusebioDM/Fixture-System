using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace EirinDuran.Entities
{
    public class UserMapper
    {
        public UserEntity Map(User user)
        {
            return new UserEntity()
            {
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Password = user.Password,
                Mail = user.Mail,
                Role = user.Role
            };
        }

        public User Map(UserEntity entity)
        {
            return new User(role: entity.Role, userName: entity.UserName, name: entity.Name, surname: entity.Surname, password: entity.Password, mail: entity.Mail);
        }
    }
}
