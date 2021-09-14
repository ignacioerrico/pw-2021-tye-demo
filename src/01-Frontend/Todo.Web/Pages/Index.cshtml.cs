using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Todo.Web.Business;
using Todo.Web.Entities;

namespace Todo.Web.Pages
{
    public class IndexModel : PageModel
    {
        public List<TodoNote> TodoNotes { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ShowPast { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ShowCompleted { get; set; }

        public async Task OnGetAsync([FromServices] IIndexFacade indexFacade)
        {
            if (Request.Query.ContainsKey("ShowPast")) // Is it in the query string?
            {
                ShowPast = bool.Parse(Request.Query["ShowPast"]);
                Response.Cookies.Append("ShowPast", ShowPast.ToString());
            }
            else if (Request.Cookies.ContainsKey("ShowPast")) // Is it in the cookie?
                ShowPast = bool.Parse(Request.Cookies["ShowPast"]);

            if (Request.Query.ContainsKey("ShowCompleted")) // Is it in the query string?
            {
                ShowCompleted = bool.Parse(Request.Query["ShowCompleted"]);
                Response.Cookies.Append("ShowCompleted", ShowCompleted.ToString());
            }
            else if (Request.Cookies.ContainsKey("ShowCompleted")) // Is it in the cookie?
                ShowCompleted = bool.Parse(Request.Cookies["ShowCompleted"]);

            TodoNotes = await indexFacade.GetAllAsync(includeDeleted: false, ShowPast, ShowCompleted);
        }

        public IActionResult OnPost()
        {
            Response.Cookies.Append("ShowPast", ShowPast.ToString());
            Response.Cookies.Append("ShowCompleted", ShowCompleted.ToString());

            return RedirectToPage(new { ShowPast, ShowCompleted });
        }
    }
}
