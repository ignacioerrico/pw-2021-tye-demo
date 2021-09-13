using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Todo.Web.DataAccess.Repositories;

namespace Todo.Web.Pages.Action
{
    public class DeleteModel : PageModel
    {
        private readonly ITodoRepository _todoRepository;

        public DeleteModel(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public IActionResult OnGet(int todoNoteId)
        {
            if (_todoRepository.Delete(todoNoteId))
                return RedirectToPage("/Index");

            return Page();
        }
    }
}
