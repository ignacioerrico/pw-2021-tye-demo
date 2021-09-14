using System.Collections.Generic;
using System.Threading.Tasks;
using Words.Grpc;

namespace Todo.Api.Grpc
{
    public class WordsGrpcService
    {
        private readonly Words.Grpc.Words.WordsClient _client;

        public WordsGrpcService(Words.Grpc.Words.WordsClient client)
        {
            _client = client;
        }

        public async Task<int> AddKeywordsAsync(IEnumerable<string> keywords)
        {
            var addKeywordsRequest = new AddKeywordsRequest { Words = { keywords } };
            var response = await _client.AddKeywordsAsync(addKeywordsRequest);

            return response.WordsAdded;
        }

        public async Task<int> GetFrequencyAsync(string word)
        {
            var getFrequenciesRequest = new GetFrequencyRequest { Word = word };
            var response = await _client.GetFrequencyAsync(getFrequenciesRequest);

            return response.Frequency;
        }
    }
}
