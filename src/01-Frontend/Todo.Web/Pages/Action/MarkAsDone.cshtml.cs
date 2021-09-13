using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Todo.Web.DataAccess.Repositories;

namespace Todo.Web.Pages.Action
{
    public class MarkAsDoneModel : PageModel
    {
        private readonly ITodoRepository _todoRepository;

        public MarkAsDoneModel(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public IActionResult OnGet(int todoNoteId)
        {
            if (_todoRepository.MarkAsDone(todoNoteId))
                return RedirectToPage("/Index");

            return Page();
        }
    }
}
