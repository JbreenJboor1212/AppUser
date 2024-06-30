﻿using System.ComponentModel.DataAnnotations;

namespace AppUser.Dto.comment
{
    public class CreateCommentDto
    {
        //Validation
        [Required]
        [MinLength(5,ErrorMessage = "Title must be 5 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;

        //Validation
        [Required]
        [MinLength(5, ErrorMessage = "Content must be 5 characters")]
        [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}
