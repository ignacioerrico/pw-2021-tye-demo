using System;

namespace Todo.Api.Entities
{
    public class TodoNote
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime DueBy { get; set; } = DateTime.UtcNow;
        public bool Accomplished { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}