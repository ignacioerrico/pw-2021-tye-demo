using System.Threading.Tasks;
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

        public async Task<IActionResult> OnPostAsync([FromServices] IAddFacade addFacade)
        {
            if (!ModelState.IsValid)
                return Page();

            await addFacade.CreateNewAsync(TodoNote);

            await addFacade.CacheWordsAsync(TodoNote.Text);

            return RedirectToPage("./Index");
        }
    }
}
