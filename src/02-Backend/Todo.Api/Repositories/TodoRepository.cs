using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Api.Entities;

namespace Todo.Api.Repositories
{
    public interface ITodoRepository
    {
        IEnumerable<TodoNote> GetAll(bool includeDeleted = false, bool includePast = false, bool includeCompleted = false);
        TodoNote GetById(int id);
        TodoNote AddNew(TodoNote todoNote);
        TodoNote MarkAsDone(int id);
        TodoNote Delete(int id);
        TodoNote UpdateExisting(int id, TodoNote todoNote);
    }

    public class TodoRepository : ITodoRepository
    {
        private readonly List<TodoNote> _todoNotes = new List<TodoNote>
        {
            new TodoNote
            {
                Id = 1,
                Text = "Prepare the presentation on Tye (from the API!)",
                DueBy = new DateTime(2021, 9, 15),
                Accomplished = false
            },
            new TodoNote
            {
                Id = 2,
                Text = "Plan vacation to Costa Rica (from the API!)",
                DueBy = new DateTime(2021, 9, 16),
                Accomplished = false
            },
            new TodoNote
            {
                Id = 3,
                Text = "Write book on microservices (from the API!)",
                DueBy = new DateTime(2021, 10, 1),
                Accomplished = false
            },
            new TodoNote
            {
                Id = 4,
                Text = "Plan vacation to Uzbekistan (from the API!)",
                DueBy = new DateTime(2021, 10, 2),
                Accomplished = false
            }
        };

        public IEnumerable<TodoNote> GetAll(bool includeDeleted = false, bool includePast = false, bool includeCompleted = false)
        {
            IEnumerable<TodoNote> todoNotes = _todoNotes ?? new List<TodoNote>();
            todoNotes = includeDeleted ? todoNotes : todoNotes.Where(tn => !tn.IsDeleted);
            todoNotes = includeCompleted ? todoNotes : todoNotes.Where(tn => !tn.Accomplished);
            todoNotes = includePast ? todoNotes : todoNotes.Where(tn => tn.DueBy >= DateTime.Today);

            todoNotes = todoNotes.OrderBy(tn => tn.DueBy);

            var result = todoNotes.ToList();
            return result;
        }

        public TodoNote GetById(int id) => _todoNotes.SingleOrDefault(tn => tn.Id == id);

        public TodoNote AddNew(TodoNote todoNote)
        {
            todoNote.Id = _todoNotes.Select(tn => tn.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;
            _todoNotes.Add(todoNote);

            return todoNote;
        }

        public TodoNote UpdateExisting(int id, TodoNote todoNote)
        {
            var todoNoteToUpdate = GetById(id);
            if (todoNoteToUpdate is null)
                return null;

            todoNoteToUpdate.Text = todoNote.Text;
            todoNoteToUpdate.DueBy = todoNote.DueBy;
            return todoNoteToUpdate;
        }

        public TodoNote MarkAsDone(int id)
        {
            var todoNoteToMarkAsDone = GetById(id);
            if (todoNoteToMarkAsDone is null)
                return null;

            todoNoteToMarkAsDone.Accomplished = true;
            return todoNoteToMarkAsDone;
        }

        public TodoNote Delete(int id)
        {
            var todoNoteToDelete = GetById(id);
            if (todoNoteToDelete is null)
                return null;

            todoNoteToDelete.IsDeleted = true;
            return todoNoteToDelete;
        }
    }
}
