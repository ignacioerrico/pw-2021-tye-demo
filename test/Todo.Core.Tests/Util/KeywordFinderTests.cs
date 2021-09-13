using System.Linq;
using NUnit.Framework;
using Todo.Core.Util;

namespace Todo.Core.Tests.Util
{
    public class KeywordFinderTests
    {
        [TestCase("Prepare the presentation on Tye", 3)] // Adds only "prepare", "presentation" and "tye" to the cache
        [TestCase("The world is your oyster", 2)] // Adds only "world" and "oyster" to the cache
        [TestCase("Buy the new single because the new single is new", 3)] // Adds only "buy", "new" and "single" to the cache
        public void GetKeywords_SomeText_ReturnsUniqueKeywords(string text, int uniqueKeywords)
        {
            // Arrange
            var keywordFinder = new KeywordFinder();

            // Act
            var keywords = keywordFinder.GetKeywords(text).ToList();

            // Assert
            Assert.AreEqual(keywords.Count, uniqueKeywords);
        }
    }
}