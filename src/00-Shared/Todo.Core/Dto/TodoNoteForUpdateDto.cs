using System;

namespace Todo.Core.Dto
{
    public class TodoNoteForUpdateDto
    {
        public string Text { get; set; }
        public DateTime DueBy { get; set; }
    }
}
