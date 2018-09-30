using EirinDuran.IServices.DTOs;
using EirinDuran.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EirinDuran.WebApi.Mappers
{
    public static class UserMapper
    {
        public static UserDTO Map(UserModelIn modelIn)
        {
            return new UserDTO()
            {
                UserName = modelIn.UserName,
                Name = modelIn.Name,
                Surname = modelIn.Surname,
                Password = modelIn.Password,
                Mail = modelIn.Mail,
                IsAdmin = modelIn.IsAdmin
            };
        }

        public static UserDTO Map(UserUpdateModelIn modelIn)
        {
            return new UserDTO()
            {
                Name = modelIn.Name,
                Surname = modelIn.Surname,
                Password = modelIn.Password,
                Mail = modelIn.Mail,
            };
        }
    }
}
