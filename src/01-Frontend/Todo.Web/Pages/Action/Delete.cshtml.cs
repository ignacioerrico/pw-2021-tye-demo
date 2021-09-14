using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Todo.Web.Pages.Action
{
    public class DeleteModel : PageModel
    {
        private readonly TodoHttpClient _httpClient;

        public DeleteModel(TodoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync(int todoNoteId)
        {
            if (await _httpClient.DeleteAsync(todoNoteId))
                return RedirectToPage("/Index");

            return Page();
        }
    }
}
