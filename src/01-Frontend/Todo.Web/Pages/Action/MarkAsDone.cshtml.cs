using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Todo.Web.Pages.Action
{
    public class MarkAsDoneModel : PageModel
    {
        private readonly TodoHttpClient _httpClient;

        public MarkAsDoneModel(TodoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync(int todoNoteId)
        {
            if (await _httpClient.MarkAsDoneAsync(todoNoteId))
                return RedirectToPage("/Index");

            return Page();
        }
    }
}
