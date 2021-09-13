using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Todo.Web.Business;
using Todo.Web.Entities;

namespace Todo.Web.Pages
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public TodoNote TodoNote { get; set; }

        public NothingToDo NothingToDo { get; set; }

        public void OnGet(NothingToDo nothingToDo = NothingToDo.NotSet)
        {
            NothingToDo = nothingToDo;

            TodoNote = nothingToDo == NothingToDo.SecondTry
                ? new TodoNote { Text = "Write my first TODO note." }
                : new TodoNote();
        }

        public IActionResult OnPost([FromServices] IAddFacade addFacade)
        {
            if (!ModelState.IsValid)
                return Page();

            addFacade.CreateNew(TodoNote);

            addFacade.CacheWords(TodoNote.Text);

            return RedirectToPage("./Index");
        }
    }
}
