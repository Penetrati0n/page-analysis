using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace page_analysis
{
    public class Filter
    {
        private readonly Regex _textRegex;
        private readonly Regex _removeRegex;
        private readonly char[] _separators;

        public Filter(string textPattern = "(?<=>)[\\s\\S]*?(?=<)",
            string removePattern = "(?<=(<script)|(<style)|(<!--))([\\s\\S]*?)(?=(\\/script)|(\\/style)|(-->))")
        {
            _textRegex = new Regex(textPattern);
            _removeRegex = new Regex(removePattern);
            _separators = new char[]
            {
                '\n', '\r', '\t', ' ', '~', '!', '@', '#', '$', '%', '^', '&', '*',
                '(', ')', '_', '+', '=', '-', '№', '[', ']', '{', '}', ';', ':', '\'',
                '"', '/', '?', '.', ',', '<', '>', '|', '\\', '«', '»'
            };
        }

        public IEnumerable<string> GetWords(string text)
        {
            text = RemoveInterference(text);

            var words = _textRegex.Matches(text)
                .SelectMany(m => m.Value.Split(_separators))
                .Where(w => IsCorrectWord(w));

            return words;
        }

        private string RemoveInterference(string text)
        {
            StringBuilder pageBuilder = new StringBuilder(text);
            var removable = _removeRegex.Matches(text);

            foreach (Match m in removable)
            {
                pageBuilder.Replace(m.Value, "");
            }

            return pageBuilder.ToString();
        }

        private static bool IsCorrectWord(string word)
        {
            return !(string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word) || IsNumber(word));
        }

        private static bool IsNumber(string word)
        {
            foreach (char let in word)
            {
                if (!char.IsDigit(let))
                    return false;
            }

            return true;
        }
    }
}
