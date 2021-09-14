using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Words.Grpc.Cache
{
    public interface ICacheWords
    {
        Task Add(string word);
        Task<int> GetFrequency(string word);
    }

    public class CacheWords : ICacheWords
    {
        private readonly IDistributedCache _redisCache;

        public CacheWords(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task Add(string word)
        {
            var frequencyJson = await _redisCache.GetStringAsync(word);

            string frequency;
            if (string.IsNullOrWhiteSpace(frequencyJson))
            {
                // Not found in the cache -> add it.
                frequency = JsonSerializer.Serialize(1);
            }
            else
            {
                // Increment the frequency
                var currentFrequency = JsonSerializer.Deserialize<int>(frequencyJson);
                frequency = JsonSerializer.Serialize(++currentFrequency);
            }

            await _redisCache.SetStringAsync(word, frequency);
        }

        public async Task<int> GetFrequency(string word)
        {
            var frequencyJson = await _redisCache.GetStringAsync(word);
            return string.IsNullOrWhiteSpace(frequencyJson) ? 0 : JsonSerializer.Deserialize<int>(frequencyJson);
        }
    }
}
