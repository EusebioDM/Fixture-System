using System;

namespace SilverFixture.IServices.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get;  set; }

        public string UserName { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Message { get; set; }
        
        public string EncounterId { get; set; }
    }
}