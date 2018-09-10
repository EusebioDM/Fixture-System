﻿using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace EirinDuran.Entities.Mappers
{
    internal class UserMapper
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

        public void Update(User source, UserEntity destination)
        {
            destination.UserName = source.UserName;
            destination.Name = source.Name;
            destination.Surname = source.Surname;
            destination.Password = source.Password;
            destination.Mail = source.Mail;
            destination.Role = source.Role;
        }
    }
}
