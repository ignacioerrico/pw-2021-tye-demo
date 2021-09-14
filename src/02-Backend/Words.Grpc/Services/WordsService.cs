using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Words.Grpc.Cache;

namespace Words.Grpc.Services
{
    public class WordsService : Words.WordsBase
    {
        private readonly ICacheWords _cacheWords;
        private readonly ILogger<WordsService> _logger;

        public WordsService(ICacheWords cacheWords, ILogger<WordsService> logger)
        {
            _cacheWords = cacheWords;
            _logger = logger;
        }

        public override Task<AddKeywordsResponse> AddKeywords(AddKeywordsRequest request, ServerCallContext context)
        {
            foreach (var word in request.Words)
                _cacheWords.Add(word);

            _logger.LogInformation($"{request.Words.Count} word(s) added to the cache.");

            var response = new AddKeywordsResponse
            {
                WordsAdded = request.Words.Count
            };

            return Task.FromResult(response);
        }

        public override async Task<GetFrequencyResponse> GetFrequency(GetFrequencyRequest request, ServerCallContext context)
        {
            var frequency = await _cacheWords.GetFrequency(request.Word);

            var response = new GetFrequencyResponse
            {
                Frequency = frequency
            };

            return response;
        }
    }
}
