using System;
using Todo.Web.Entities;

namespace Todo.Web.Extensions
{
    public static class HumanizerExtensions
    {
        public static string Humanize(this DateTime dateTime)
        {
            var dayDifference = (dateTime.Date - DateTime.Today).Days;
            
            if (dayDifference < 0)
                return "Too late to get this done!";

            if (dayDifference > 30)
                return "You can relax for this one.";

            string expression = dayDifference switch
            {
                0 => "today",
                1 => "tomorrow",
                _ => $"in {dayDifference} days"
            };

            return $"Due {expression}";
        }

        public static string GetTableRowStyle(this TodoNote todoNote)
        {
            if (todoNote.Accomplished)
                return "table-secondary";
            
            var dayDifference = (todoNote.DueBy.Date - DateTime.Today).Days;

            if (dayDifference <= 0)
                return "table-danger";
            
            if (dayDifference <= 5)
                return "table-warning";

            if (dayDifference <= 30)
                return "table-success";

            return "table-light";
        }
    }
}
