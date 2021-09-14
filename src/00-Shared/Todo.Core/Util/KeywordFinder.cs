using System;
using System.Collections.Generic;
using System.Linq;

namespace Todo.Core.Util
{
    public interface IKeywordFinder
    {
        IEnumerable<string> GetKeywords(string text);
        IEnumerable<string> GetKeywords(List<string> words);
    }

    public class KeywordFinder : IKeywordFinder
    {
        private readonly string[] _wordsToRemove =
        {
            // Articles
            "the", "a", "an",
            
            // Pronouns
            "i", "you", "he", "she", "it", "we", "they",

            // Possessives
            "my", "mine", "your", "yours", "his", "her", "its", "our", "ours", "their", "theirs",

            // Connectors
            "and", "or", "but", "however", "nevertheless", "because",

            // Common Adverbs
            "never", "always",

            // Common verbs
            "be", "is", "are", "been", "do", "does", "done", "have", "has",
            "isn't", "aren't", "don't", "doesn't", "haven't", "hasn't",

            // Modal verbs
            "can", "could", "may", "might", "shall", "should", "will", "would", "must", "dare", "need", "want",
            "can't", "couldn't", "shouldn't", "won't", "wouldn't", "mustn't", "needn't",

            // Negation
            "no", "not", "ain't",
            
            // Prepositions
            "aboard", "about", "above", "across", "after", "against", "along",
            "amid", "among", "anti", "around", "as", "at", "before", "behind", "below", "beneath", "beside",
            "besides", "between", "beyond", "but", "by", "concerning", "considering", "despite", "down", "during",
            "except", "excepting", "excluding", "following", "for", "from", "in", "inside", "into", "like", "minus",
            "near", "of", "off", "on", "onto", "opposite", "outside", "over", "past", "per", "plus", "regarding",
            "round", "save", "since", "than", "through", "to", "toward", "towards", "under", "underneath", "unlike",
            "until", "up", "upon", "versus", "via", "with", "within", "without"
        };

        public IEnumerable<string> GetKeywords(string text)
        {
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            return GetKeywords(words);
        }

        public IEnumerable<string> GetKeywords(List<string> words)
        {
            var keywords = words
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => w.ToLowerInvariant())
                .Distinct()
                .Where(w => !_wordsToRemove.Contains(w));

            return keywords;
        }
    }
}
