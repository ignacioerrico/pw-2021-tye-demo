using Todo.Core.Util;
using Todo.Web.Cache;
using Todo.Web.DataAccess.Repositories;
using Todo.Web.Entities;

namespace Todo.Web.Business
{
    public interface IAddFacade
    {
        void CreateNew(TodoNote todoNote);
        void CacheWords(string todoNoteText);
    }

    public class AddFacade : IAddFacade
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ICacheWords _cacheWords;
        private readonly IKeywordFinder _keywordFinder;

        public AddFacade(ITodoRepository todoRepository, ICacheWords cacheWords, IKeywordFinder keywordFinder)
        {
            _todoRepository = todoRepository;
            _cacheWords = cacheWords;
            _keywordFinder = keywordFinder;
        }

        public void CreateNew(TodoNote todoNote)
        {
            _todoRepository.AddNew(todoNote);
        }

        public void CacheWords(string todoNoteText)
        {
            var wordsToCache = _keywordFinder.GetKeywords(todoNoteText);

            foreach (var word in wordsToCache)
                _cacheWords.Add(word);
        }
    }
}
