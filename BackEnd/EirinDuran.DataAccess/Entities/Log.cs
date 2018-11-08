using System;
using EirinDuran.GenericEntityRepository;

namespace EirinDuran.DataAccess.Entities
{
    public class Log : IEntity<Log>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public DateTime DateTime { get; private set; }

        public Log()
        {
            DateTime = DateTime.Now;
            Id = Guid.NewGuid();
        }

        public Log(string userName, string action) : this()
        {
            this.UserName = userName;
            Action = action;
        }

        public void UpdateWith(Log model)
        {
            UserName = model.UserName;
            Action = model.Action;
            DateTime = model.DateTime;
            Id = model.Id;
        }

        public Log ToModel()
        {
            return this;
        }
    }
}