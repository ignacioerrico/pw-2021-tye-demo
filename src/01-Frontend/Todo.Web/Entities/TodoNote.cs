using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Web.Entities
{
    public class TodoNote
    {
        public int Id { get; set; }

        [Display(Name = "What do you wanna get done?")]
        [CustomValidation(typeof(CustomValidationMethods), nameof(CustomValidationMethods.TextHasLowerAndUpperBounds))]
        public string Text { get; set; } = string.Empty;

        [Required, DataType(DataType.Date)]
        [Display(Name = "What's the latest you can sort it out?")]
        [CustomValidation(typeof(CustomValidationMethods), nameof(CustomValidationMethods.DateMustBePresentOrFuture))]
        public DateTime DueBy { get; set; } = DateTime.UtcNow;

        public bool Accomplished { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}