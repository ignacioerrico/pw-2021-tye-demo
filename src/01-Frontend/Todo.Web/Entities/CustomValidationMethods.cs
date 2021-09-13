using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Web.Entities
{
    public class CustomValidationMethods
    {
        public static ValidationResult TextHasLowerAndUpperBounds(string text)
        {
            if (text is null)
                return new ValidationResult("So you don't really wanna do anything, huh?");

            if (text.Length < 3)
                return new ValidationResult("We both know that's not something you can get done.");

            if (text.Length > 255)
                return new ValidationResult("Hmm... that's a bit of a mouthful. The shorter, the sooner you'll get that done.");

            return ValidationResult.Success;
        }

        public static ValidationResult DateMustBePresentOrFuture(DateTime date)
        {
            return date.Date >= DateTime.Today
                ? ValidationResult.Success
                : new ValidationResult("You won't get that done if you haven't done it yet...");
        }
    }
}