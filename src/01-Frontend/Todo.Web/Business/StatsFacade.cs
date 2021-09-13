using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Web.Business.Models;
using Todo.Web.Cache;
using Todo.Web.DataAccess.Repositories;

namespace Todo.Web.Business
{
    public interface IStatsFacade
    {
        StatsModel CalculateStats();
        IEnumerable<KeyValuePair<string, int>> GetFrequencies();
    }

    public class StatsFacade : IStatsFacade
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ICacheWords _cacheWords;

        public StatsFacade(ITodoRepository todoRepository, ICacheWords cacheWords, IAddFacade addFacade)
        {
            _todoRepository = todoRepository;
            _cacheWords = cacheWords;

            // TODO: This is ugly.  I'm injecting and IAddFacade just to crunch the hardcoded data.  But that's the point, I'm hardcoding data.
            foreach (var todoNote in todoRepository.TodoNotes)
            {
                addFacade.CacheWords(todoNote.Text);
            }
        }

        public StatsModel CalculateStats()
        {
            var result = new StatsModel();
            
            var todoNotes = _todoRepository.GetAll(includeDeleted: true, includePast: true, includeCompleted: true)
                .ToList();

            var allTimeTotal = todoNotes.Count;
            result.AllTimeTotal = allTimeTotal;

            var accomplised = todoNotes.Count(tn => tn.Accomplished);
            result.AllTimePercentageDone = allTimeTotal > 0 ? (double)accomplised / allTimeTotal : 0;

            var deleted = todoNotes.Count(tn => tn.IsDeleted);
            result.AllTimePercentageProcrastinated = allTimeTotal > 0 ? (double)deleted / allTimeTotal : 0;

            var futureTodoNotes = todoNotes.Where(tn => tn.DueBy >= DateTime.Today).ToList();

            var futureTasksTotal = futureTodoNotes.Count;
            result.FutureTasksTotal = futureTasksTotal;

            var futureTasksAccomplised = futureTodoNotes.Count(tn => tn.Accomplished);
            result.FutureTasksPercentageDone = futureTasksTotal > 0 ? (double)futureTasksAccomplised / futureTasksTotal : 0;

            var futureTasksDeleted = futureTodoNotes.Count(tn => tn.IsDeleted);
            result.FutureTasksPercentageProcrastinated = futureTasksTotal > 0 ? (double)futureTasksDeleted / futureTasksTotal : 0;

            return result;
        }

        public IEnumerable<KeyValuePair<string, int>> GetFrequencies()
        {
            return _cacheWords.GetFrequencies()
                .OrderByDescending(f => f.Value);
        }
    }
}
