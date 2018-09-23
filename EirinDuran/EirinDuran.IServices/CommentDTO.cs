using System;

namespace EirinDuran.IServices
{
    public class CommentDTO
    {
        public Guid Id { get;  set; }

        public UserDTO User { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Message { get; set; }
    }
}