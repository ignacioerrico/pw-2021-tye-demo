using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Todo.Web.Business;

namespace Todo.Web.Pages
{
    public class StatsModel : PageModel
    {
        public int AllTimeTotal { get; set; }
        public double AllTimePercentageDone { get; set; }
        public double AllTimePercentageProcrastinated { get; set; }
        
        public int FutureTasksTotal { get; set; }
        public double FutureTasksPercentageDone { get; set; }
        public double FutureTasksPercentageProcrastinated { get; set; }

        public IEnumerable<KeyValuePair<string, int>> WordFrequencies { get; set; }

        public async Task OnGetAsync([FromServices] IStatsFacade statsFacade)
        {
            var stats = await statsFacade.CalculateStatsAsync();

            AllTimeTotal = stats.AllTimeTotal;
            AllTimePercentageDone = stats.AllTimePercentageDone;
            AllTimePercentageProcrastinated = stats.AllTimePercentageProcrastinated;

            FutureTasksTotal = stats.FutureTasksTotal;
            FutureTasksPercentageDone = stats.FutureTasksPercentageDone;
            FutureTasksPercentageProcrastinated = stats.FutureTasksPercentageProcrastinated;

            WordFrequencies = await statsFacade.GetFrequenciesAsync();
        }
    }
}
