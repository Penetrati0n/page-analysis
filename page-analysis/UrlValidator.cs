using System.Text.RegularExpressions;

namespace page_analysis
{
    public class UrlValidator
    {
        private readonly Regex _urlRegex;

        public UrlValidator(string urlPattern = "(?<http>(https?:\\/\\/)|)(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b")
        {
            _urlRegex = new Regex(urlPattern);
        }

        public bool IsValid(string url)
        {
            return _urlRegex.IsMatch(url);
        }

        public string GetValidUrl(string url)
        {
            Match match = _urlRegex.Match(url);

            if (string.IsNullOrEmpty(match.Groups["http"].Value))
                url = "http://" + url;

            return url;
        }
    }
}
