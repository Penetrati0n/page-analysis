using System.Linq;
using Xunit;

namespace page_analysis.Tests
{
    public class FilterTest
    {
        [Theory]
        [InlineData("<a>Hello world!</a>")]
        public void GetWords_ActionExecutes_ReturnsWords(string input)
        {
            var filter = new Filter();

            var result = filter.GetWords(input);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("<a>Hello world!</a>", 2)]
        public void GetWords_ActionExecutes_ReturnsExactNumberOfWords(string input, int expected)
        {
            var filter = new Filter();

            var words = filter.GetWords(input);
            var result = words.Count();

            Assert.Equal(expected, result);
        }
    }
}
