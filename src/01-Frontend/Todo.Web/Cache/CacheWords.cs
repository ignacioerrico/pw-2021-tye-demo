using System.Collections.Generic;

namespace Todo.Web.Cache
{
    public interface ICacheWords
    {
        void Add(string word);
        int GetFrequency(string word);
        Dictionary<string, int> GetFrequencies();
    }

    public class CacheWords : ICacheWords
    {
        private readonly Dictionary<string, int> _words = new Dictionary<string, int>();

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

        public Dictionary<string, int> GetFrequencies()
        {
            return _words;
        }
    }
}
