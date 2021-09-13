using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Dto
{
    public class TodoNoteDto
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "Text must be at least three characters.")]
        [MaxLength(255, ErrorMessage = "Text cannot exceed 255 characters.")]
        public string Text { get; set; }
        [Required(ErrorMessage = "DueBy must be specified.")]
        public DateTime DueBy { get; set; }
        public bool Accomplished { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
