using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace api.Models
{
    public class AddPhotoModel
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
