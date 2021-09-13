using System.Collections.Generic;
using System.Linq;
using Todo.Web.DataAccess.Repositories;
using Todo.Web.Entities;

namespace Todo.Web.Business
{
    public interface IIndexFacade
    {
        List<TodoNote> GetAll(bool includeDeleted, bool includePast, bool includeCompleted);
    }

    public class IndexFacade : IIndexFacade
    {
        private readonly ITodoRepository _todoRepository;

        public IndexFacade(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public List<TodoNote> GetAll(bool includeDeleted, bool includePast, bool includeCompleted)
        {
            return _todoRepository.GetAll(includeDeleted, includePast, includeCompleted).ToList();
        }
    }
}
