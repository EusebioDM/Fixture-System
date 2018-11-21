using System;
using EirinDuran.GenericEntityRepository;
using SilverFixture.IServices.DTOs;

namespace SilverFixture.DataAccess.Entities
{
    public class LogEntity : IEntity<LogDTO>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public DateTime DateTime { get; set; }

        public LogEntity()
        {
            DateTime = DateTime.Now;
            Id = Guid.NewGuid();
        }

        public LogEntity(string userName, string action) : this()
        {
            this.UserName = userName;
            Action = action;
        }

        public void UpdateWith(LogDTO model)
        {
            Action = model.Action;
            UserName = model.UserName;
            if (model.DateTime != new DateTime())
            {
                DateTime = model.DateTime;
            }
        }

        LogDTO IEntity<LogDTO>.ToModel()
        {
            return new LogDTO()
            {
                DateTime = DateTime,
                Action = Action,
                UserName = UserName
            };
        }
    }
}