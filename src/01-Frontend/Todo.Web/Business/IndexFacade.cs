using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Web.Entities;

namespace Todo.Web.Business
{
    public interface IIndexFacade
    {
        Task<List<TodoNote>> GetAllAsync(bool includeDeleted, bool includePast, bool includeCompleted);
    }

    public class IndexFacade : IIndexFacade
    {
        private readonly TodoHttpClient _httpClient;

        public IndexFacade(TodoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TodoNote>> GetAllAsync(bool includeDeleted, bool includePast, bool includeCompleted)
        {
            var todoNotes = await _httpClient.GetAllAsync(includeDeleted, includePast, includeCompleted);
            return todoNotes.ToList();
        }
    }
}
