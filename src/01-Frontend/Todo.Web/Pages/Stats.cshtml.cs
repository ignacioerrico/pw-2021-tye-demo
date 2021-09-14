using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Todo.Web.Business;
using Todo.Web.Entities;

namespace Todo.Web.Pages
{
    public class StatsModel : PageModel
    {
        private readonly IStatsFacade _statsFacade;

        public StatsModel(IStatsFacade statsFacade)
        {
            _statsFacade = statsFacade;
        }

        public int AllTimeTotal { get; set; }
        public double AllTimePercentageDone { get; set; }
        public double AllTimePercentageProcrastinated { get; set; }
        
        public int FutureTasksTotal { get; set; }
        public double FutureTasksPercentageDone { get; set; }
        public double FutureTasksPercentageProcrastinated { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "You need to input a word")]
        [Display(Name = "Input a word to check how often you've used it")]
        [MaxLength(255)]
        public string Word { get; set; }
        
        public string WordSearched { get; set; }

        public int Frequency { get; set; }

        public async Task OnGetAsync()
        {
            var stats = await _statsFacade.CalculateStatsAsync();

            AllTimeTotal = stats.AllTimeTotal;
            AllTimePercentageDone = stats.AllTimePercentageDone;
            AllTimePercentageProcrastinated = stats.AllTimePercentageProcrastinated;

            FutureTasksTotal = stats.FutureTasksTotal;
            FutureTasksPercentageDone = stats.FutureTasksPercentageDone;
            FutureTasksPercentageProcrastinated = stats.FutureTasksPercentageProcrastinated;
        }

        public async Task OnPostAsync()
        {
            Frequency = await _statsFacade.GetFrequencyAsync(Word);
            WordSearched = Word;
            Word = string.Empty;
        }
    }
}
