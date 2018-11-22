using SilverFixture.IServices.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverFixture.WebApi.Models
{
    public class CommentModelIn
    {
        [Required]
        public string Message { get; set; }
    }
}