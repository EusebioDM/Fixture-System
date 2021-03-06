﻿using SilverFixture.Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilverFixture.DataAccess.Entities
{
    public class EntityMapper
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
