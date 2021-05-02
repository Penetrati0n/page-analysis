using Xunit;

namespace page_analysis.Tests
{
    public class UrlValidatorTest
    {
        [Theory]
        [InlineData("google.com", true)]
        [InlineData("google.comcomcom", false)]
        public void IsValid_ActionExecutes_TrueOrFalse(string input, bool expected)
        {
            var validator = new UrlValidator();

            var result = validator.IsValid(input);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("google.com", "http://google.com")]
        [InlineData("http://google.com", "http://google.com")]
        public void GetValidUrl_ActionExecutes_ReturnValidUrl(string input, string expected)
        {
            var validator = new UrlValidator();

            var result = validator.GetValidUrl(input);

            Assert.Equal(expected, result);
        }
    }
}
