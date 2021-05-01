using System;
using System.Collections.Generic;
using System.Linq;

namespace page_analysis
{
    public class Presenter : IPresenter
    {
        private readonly UrlValidator _urlValidator;

        public Presenter()
        {
            _urlValidator = new UrlValidator();
        }

        public void HelloMessage()
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Welcome to the page analysis program!");
            Console.WriteLine("-------------------------------------");
        }

        public void InputUrl(ref string url)
        {
            Console.Write("Enter the URL of the page: ");
            
            url = Console.ReadLine();

            while (!_urlValidator.IsValid(url))
            {
                Console.Write("Your URL is not correct. Please enter a valid URL: ");
                url = Console.ReadLine();
            }

            url = _urlValidator.GetValidUrl(url);
        }

        public void PrintStats(IEnumerable<KeyValuePair<string, int>> stats)
        {
            if (stats == null)
            {
                Console.WriteLine("An error has occurred :(");
                return;
            }

            Console.WriteLine("----------------Stats----------------");

            int maxLen = stats.Max(x => x.Key.Length);

            foreach (var s in stats)
            {
                Console.WriteLine($"{WordWithIdent(s.Key, maxLen)} : {s.Value}");
            }

            Console.WriteLine("-------------------------------------");
        }

        private static string WordWithIdent(string word, int maxLen)
        {
            while (word.Length < maxLen)
            {
                word = " " + word;
            }

            return word;
        }
    }
}
