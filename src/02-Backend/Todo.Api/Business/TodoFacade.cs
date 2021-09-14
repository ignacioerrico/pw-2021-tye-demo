using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Mapster;
using Todo.Api.Entities;
using Todo.Api.Grpc;
using Todo.Api.Repositories;
using Todo.Core.Dto;
using Todo.Core.Util;

namespace Todo.Api.Business
{
    public interface ITodoFacade
    {
        Task<int> AddKeywordsAsync(List<string> words);
        Task<int> GetFrequencyAsync(string word);
        IEnumerable<TodoNoteDto> GetAll(bool includeDeleted, bool includePast, bool includeCompleted);
        TodoNoteDto GetById(int id);
        StatsDto GetStats();
        TodoNoteDto CreateNew(TodoNoteDto todoNoteDto);
        TodoNote MarkAsDone(int id);
        TodoNote UpdateExisting(int id, TodoNoteForUpdateDto todoNoteForUpdateDto);
        TodoNote Delete(int id);
    }

    public class TodoFacade : ITodoFacade
    {
        private readonly ITodoRepository _todoRepository;
        private readonly WordsGrpcService _grpcService;
        private readonly IKeywordFinder _keywordFinder;

        public TodoFacade(ITodoRepository todoRepository, WordsGrpcService grpcService, IKeywordFinder keywordFinder)
        {
            _todoRepository = todoRepository;
            _grpcService = grpcService;
            _keywordFinder = keywordFinder;
        }

        public async Task<int> AddKeywordsAsync(List<string> words)
        {
            var keywords = _keywordFinder.GetKeywords(words);
            var wordsAdded = await _grpcService.AddKeywordsAsync(keywords);
            return wordsAdded;
        }

        public async Task<int> GetFrequencyAsync(string word)
        {
            var frequency = await _grpcService.GetFrequencyAsync(word);
            return frequency;
        }

        public IEnumerable<TodoNoteDto> GetAll(bool includeDeleted, bool includePast, bool includeCompleted)
        {
            return _todoRepository.GetAll(includeDeleted, includePast, includeCompleted)
                .Adapt<IEnumerable<TodoNoteDto>>();
        }

        public TodoNoteDto GetById(int id)
        {
            var todoNote = _todoRepository.GetById(id);
            return todoNote?.Adapt<TodoNoteDto>();
        }

        public StatsDto GetStats()
        {
            var todoNotes = _todoRepository.GetAll(includeDeleted: true, includePast: true, includeCompleted: true)
                .ToList();

            var statsModel = new StatsDto();

            var allTimeTotal = todoNotes.Count;
            statsModel.AllTimeTotal = allTimeTotal;

            var accomplised = todoNotes.Count(tn => tn.Accomplished);
            statsModel.AllTimePercentageDone = allTimeTotal > 0 ? (double)accomplised / allTimeTotal : 0;

            var deleted = todoNotes.Count(tn => tn.IsDeleted);
            statsModel.AllTimePercentageProcrastinated = allTimeTotal > 0 ? (double)deleted / allTimeTotal : 0;

            var futureTodoNotes = todoNotes.Where(tn => tn.DueBy >= DateTime.Today).ToList();

            var futureTasksTotal = futureTodoNotes.Count;
            statsModel.FutureTasksTotal = futureTasksTotal;

            var futureTasksAccomplised = futureTodoNotes.Count(tn => tn.Accomplished);
            statsModel.FutureTasksPercentageDone = futureTasksTotal > 0 ? (double)futureTasksAccomplised / futureTasksTotal : 0;

            var futureTasksDeleted = futureTodoNotes.Count(tn => tn.IsDeleted);
            statsModel.FutureTasksPercentageProcrastinated = futureTasksTotal > 0 ? (double)futureTasksDeleted / futureTasksTotal : 0;

            return statsModel;
        }

        public TodoNoteDto CreateNew(TodoNoteDto todoNoteDto)
        {
            var newTodoNote = _todoRepository.AddNew(todoNoteDto.Adapt<TodoNote>());
            return newTodoNote.Adapt<TodoNoteDto>();
        }

        public TodoNote MarkAsDone(int id)
        {
            return _todoRepository.MarkAsDone(id);
        }

        public TodoNote UpdateExisting(int id, TodoNoteForUpdateDto todoNoteForUpdateDto)
        {
            return _todoRepository.UpdateExisting(id, todoNoteForUpdateDto.Adapt<TodoNote>());
        }

        public TodoNote Delete(int id)
        {
            return _todoRepository.Delete(id);
        }
    }
}
