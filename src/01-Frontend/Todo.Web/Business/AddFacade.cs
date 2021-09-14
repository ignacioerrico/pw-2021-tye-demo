using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Util;
using Todo.Web.Entities;

namespace Todo.Web.Business
{
    public interface IAddFacade
    {
        Task CreateNewAsync(TodoNote todoNote);
        Task CacheWordsAsync(string todoNoteText);
    }

    public class AddFacade : IAddFacade
    {
        private readonly TodoHttpClient _httpClient;
        private readonly IKeywordFinder _keywordFinder;

        public AddFacade(TodoHttpClient httpClient, IKeywordFinder keywordFinder)
        {
            _httpClient = httpClient;
            _keywordFinder = keywordFinder;
        }

        public async Task CreateNewAsync(TodoNote todoNote)
        {
            await _httpClient.AddNewAsync(todoNote);
        }

        public async Task CacheWordsAsync(string todoNoteText)
        {
            var wordsToCache = _keywordFinder.GetKeywords(todoNoteText);

            await _httpClient.AddKeywordsAsync(wordsToCache.ToList());
        }
    }
}
