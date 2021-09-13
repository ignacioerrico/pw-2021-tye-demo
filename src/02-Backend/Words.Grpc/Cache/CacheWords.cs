using System.Collections.Generic;
using System.Linq;

namespace Words.Grpc.Cache
{
    public interface ICacheWords
    {
        int Size { get; }
        void Add(string word);
        int GetFrequency(string word);
        Dictionary<string, int> GetFrequencies(int amount);
    }

    public class CacheWords : ICacheWords
    {
        private readonly Dictionary<string, int> _words = new Dictionary<string, int>();

        public int Size => _words.Count;

        public void Add(string word)
        {
            if (_words.ContainsKey(word))
                _words[word]++;
            else
                _words[word] = 1;
        }

        public int GetFrequency(string word)
        {
            return _words.ContainsKey(word) ? _words[word] : 0;
        }

        public Dictionary<string, int> GetFrequencies(int minFrequency)
        {
            if (minFrequency <= 0)
                return _words;

            return _words.Where(w => w.Value >= minFrequency).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}
