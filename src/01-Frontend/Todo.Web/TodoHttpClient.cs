using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mapster;
using Todo.Core.Dto;
using Todo.Web.Business.Models;
using Todo.Web.Entities;

namespace Todo.Web
{
    public class TodoHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public TodoHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TodoNote>> GetAllAsync(bool includeDeleted = false, bool includePast = false, bool includeCompleted = false)
        {
            var queryString = $"?includeDeleted={includeDeleted}&includePast={includePast}&includeCompleted={includeCompleted}";

            var responseMessage = await _httpClient.GetAsync($"/api/todo{queryString}");
            if (!responseMessage.IsSuccessStatusCode) return null;

            var stream = await responseMessage.Content.ReadAsStreamAsync();
            var todoNoteDto = await JsonSerializer.DeserializeAsync<List<TodoNoteDto>>(stream, _options) ?? new List<TodoNoteDto>();
            return todoNoteDto.Adapt<List<TodoNote>>();
        }

        public async Task<TodoNote> GetByIdAsync(int id)
        {
            var responseMessage = await _httpClient.GetAsync($"/api/todo{id}");
            if (!responseMessage.IsSuccessStatusCode) return null;
            
            var stream = await responseMessage.Content.ReadAsStreamAsync();
            var todoNoteDto = await JsonSerializer.DeserializeAsync<TodoNoteDto>(stream, _options) ?? new TodoNoteDto();
            return todoNoteDto.Adapt<TodoNote>();
        }

        public async Task<StatsModel> GetStatsAsync()
        {
            var responseMessage = await _httpClient.GetAsync("/api/todo/stats");
            if (!responseMessage.IsSuccessStatusCode) return null;

            var stream = await responseMessage.Content.ReadAsStreamAsync();
            var statsDto = await JsonSerializer.DeserializeAsync<StatsDto>(stream, _options) ?? new StatsDto();
            return statsDto.Adapt<StatsModel>();
        }

        public async Task<TodoNote> AddNewAsync(TodoNote todoNote)
        {
            var json = JsonSerializer.Serialize(todoNote.Adapt<TodoNoteDto>());
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync("/api/todo", data);
            if (!responseMessage.IsSuccessStatusCode) return null;

            var stream = await responseMessage.Content.ReadAsStreamAsync();
            var todoNoteDto = await JsonSerializer.DeserializeAsync<TodoNoteDto>(stream, _options) ?? new TodoNoteDto();
            return todoNoteDto.Adapt<TodoNote>();
        }

        public async Task<int> AddKeywordsAsync(List<string> words)
        {
            var json = JsonSerializer.Serialize(words);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync("/api/todo/wordfreq", data);
            if (!responseMessage.IsSuccessStatusCode) return 0;

            var stream = await responseMessage.Content.ReadAsStreamAsync();
            var frequencies = await JsonSerializer.DeserializeAsync<int>(stream, _options);
            return frequencies;
        }

        public async Task<int> GetFrequencyAsync(string word)
        {
            var responseMessage = await _httpClient.GetAsync($"/api/todo/wordfreq/{word}");
            if (!responseMessage.IsSuccessStatusCode) return 0;

            var stream = await responseMessage.Content.ReadAsStreamAsync();
            var frequency = await JsonSerializer.DeserializeAsync<int>(stream, _options);
            return frequency;
        }

        public async Task<bool> UpdateExistingAsync(TodoNote todoNote)
        {
            var json = JsonSerializer.Serialize(todoNote.Adapt<TodoNoteForUpdateDto>());
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PutAsync("/api/todo", data);
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> MarkAsDoneAsync(int id)
        {
            var json = JsonSerializer.Serialize(id.ToString());
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync("/api/todo/markdone", data);
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var responseMessage = await _httpClient.DeleteAsync($"/api/todo/{id}");
            return responseMessage.IsSuccessStatusCode;
        }
    }
}
