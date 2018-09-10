using EirinDuran.Domain.User;
using EirinDuran.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Entities
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
        private UserMapper mapper;

        public UserEntity()
        {
            mapper = new UserMapper();
        }

        public UserEntity(User user) : this()
        {
            UpdateWith(user);
        }

        public void UpdateWith(User user)
        {
            mapper.Update(user, this);
        }

        public User ToModel()
        {
            return mapper.Map(this);
        }

        public override bool Equals(object obj)
        {
            var entity = obj as UserEntity;
            return entity != null &&
                   Id == entity.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
