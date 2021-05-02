using System.Collections.Generic;
using Xunit;

namespace page_analysis.Tests
{
    public class PageInfoTest
    {
        [Theory]
        [InlineData("http://asd.asd")]
        public void GetStatistic_ActionExecutes_ReturnsNull(string input)
        {
            var pageInfo = new PageInfo();

            var result = pageInfo.GetStatistic(input, false);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("https://google.com")]
        public void GetStatistic_ActionExecutes_ReturnsStatistics(string input)
        {
            var pageInfo = new PageInfo();

            var result = pageInfo.GetStatistic(input, false);

            Assert.IsAssignableFrom<IEnumerable<KeyValuePair<string, int>>>(result);
        }
    }
}
