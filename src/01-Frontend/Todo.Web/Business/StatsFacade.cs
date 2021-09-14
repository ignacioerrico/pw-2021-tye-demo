using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Web.Business.Models;

namespace Todo.Web.Business
{
    public interface IStatsFacade
    {
        Task<StatsModel> CalculateStatsAsync();
        Task<int> GetFrequencyAsync(string word);
    }

    public class StatsFacade : IStatsFacade
    {
        private readonly TodoHttpClient _httpClient;

        public StatsFacade(TodoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StatsModel> CalculateStatsAsync()
        {
            var stats = await _httpClient.GetStatsAsync();
            return stats;
        }

        public async Task<int> GetFrequencyAsync(string word)
        {
            var frequency = await _httpClient.GetFrequencyAsync(word);
            return frequency;
        }
    }
}
