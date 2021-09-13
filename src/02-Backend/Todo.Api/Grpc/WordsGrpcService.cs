using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Util;
using Words.Grpc;

namespace Todo.Api.Grpc
{
    public class WordsGrpcService
    {
        private readonly Words.Grpc.Words.WordsClient _client;
        private readonly IKeywordFinder _keywordFinder;

        public WordsGrpcService(Words.Grpc.Words.WordsClient client, IKeywordFinder keywordFinder)
        {
            _client = client;
            _keywordFinder = keywordFinder;
        }

        public async Task<int> AddKeywordsAsync(IEnumerable<string> keywords)
        {
            var addKeywordsRequest = new AddKeywordsRequest { Words = { keywords } };
            var response = await _client.AddKeywordsAsync(addKeywordsRequest);

            return response.WordsAdded;
        }

        public async Task<Dictionary<string, int>> GetFrequenciesAsync(int minFrequency)
        {
            var getFrequenciesRequest = new GetFrequenciesRequest { MinFrequency = minFrequency };
            var response = await _client.GetFrequenciesAsync(getFrequenciesRequest);

            return response.Frequencies.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}
