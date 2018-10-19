using EirinDuran.Domain.User;
using EirinDuran.DataAccess.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.DataAccess.Entities
{
    public class UserEntity : IEntity<User>
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public Role Role { get; set; }
        public virtual ICollection<TeamUserEntity> TeamUsers { get; set; }


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
                   UserName == entity.UserName;
        }

        public override int GetHashCode()
        {
            return 404878561 + EqualityComparer<string>.Default.GetHashCode(UserName);
        }
    }
}
