using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Web.Entities;

namespace Todo.Web.DataAccess.Repositories
{
    public interface ITodoRepository
    {
        IEnumerable<TodoNote> GetAll(bool includeDeleted = false, bool includePast = false, bool includeCompleted = false);
        TodoNote GetById(int id);
        void AddNew(TodoNote todoNote);
        bool MarkAsDone(int id);
        bool Delete(int id);
        List<TodoNote> TodoNotes { get; }
    }

    public class TodoRepository : ITodoRepository
    {
        public List<TodoNote> TodoNotes { get; } = new List<TodoNote>
        {
            new TodoNote
            {
                Id = 1,
                Text = "Prepare the presentation on Tye",
                DueBy = new DateTime(2021, 9, 15),
                Accomplished = false
            },
            new TodoNote
            {
                Id = 2,
                Text = "Plan vacation to Costa Rica",
                DueBy = new DateTime(2021, 9, 16),
                Accomplished = false
            },
            new TodoNote
            {
                Id = 3,
                Text = "Write book on microservices",
                DueBy = new DateTime(2021, 10, 1),
                Accomplished = false
            },
            new TodoNote
            {
                Id = 4,
                Text = "Plan vacation to Uzbekistan",
                DueBy = new DateTime(2021, 10, 2),
                Accomplished = false
            }
        };

        public IEnumerable<TodoNote> GetAll(bool includeDeleted = false, bool includePast = false, bool includeCompleted = false)
        {
            IEnumerable<TodoNote> todoNotes = TodoNotes ?? new List<TodoNote>();
            todoNotes = includeDeleted ? todoNotes : todoNotes.Where(tn => !tn.IsDeleted);
            todoNotes = includeCompleted ? todoNotes : todoNotes.Where(tn => !tn.Accomplished);
            todoNotes = includePast ? todoNotes : todoNotes.Where(tn => tn.DueBy >= DateTime.Today);

            todoNotes = todoNotes.OrderBy(tn => tn.DueBy);

            var result = todoNotes.ToList();
            return result;
        }

        public TodoNote GetById(int id) => TodoNotes.SingleOrDefault(tn => tn.Id == id);

        public void AddNew(TodoNote todoNote)
        {
            todoNote.Id = TodoNotes.Select(tn => tn.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;
            TodoNotes.Add(todoNote);
        }

        public bool MarkAsDone(int id)
        {
            var todoNoteToMarkAsDone = GetById(id);
            if (todoNoteToMarkAsDone is null)
                return false;

            todoNoteToMarkAsDone.Accomplished = true;
            return true;
        }

        public bool Delete(int id)
        {
            var todoNoteToDelete = GetById(id);
            if (todoNoteToDelete is null)
                return false;

            todoNoteToDelete.IsDeleted = true;
            return true;
        }
    }
}
