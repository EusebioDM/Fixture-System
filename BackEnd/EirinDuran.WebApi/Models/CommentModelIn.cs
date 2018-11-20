using EirinDuran.IServices.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EirinDuran.WebApi.Models
{
    public class CommentModelIn
    {
        [Required]
        public string Message { get; set; }
    }
}