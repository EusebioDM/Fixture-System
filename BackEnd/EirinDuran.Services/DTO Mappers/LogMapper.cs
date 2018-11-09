using EirinDuran.DataAccess.Entities;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.Services.DTO_Mappers
{
    internal class LogMapper : DTOMapper<Log, LogDTO>
    {
        public override LogDTO Map(Log model)
        {
            return new LogDTO()
            {
                Action = model.Action,
                UserName = model.UserName,
                DateTime = model.DateTime
            };
        }

        protected override Log TryToMapModel(LogDTO dto)
        {
            return new Log(dto.UserName, dto.Action);
        }
    }
}