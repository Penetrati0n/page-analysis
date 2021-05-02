using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace page_analysis
{
    public class Presenter : IPresenter
    {
        private readonly UrlValidator _urlValidator;
        private readonly Logger _logger;

        public Presenter()
        {
            _urlValidator = new UrlValidator();
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void HelloMessage()
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Welcome to the page analysis program!");
            Console.WriteLine("-------------------------------------");
            _logger.Info("Print Hello Message.");
        }

        public void InputUrl(ref string url)
        {
            Console.Write("Enter the URL of the page: ");
            
            url = Console.ReadLine();

            while (!_urlValidator.IsValid(url))
            {
                _logger.Info("Get incorrect URL.");
                Console.Write("Your URL is not correct. Please enter a valid URL: ");
                url = Console.ReadLine();
            }

            url = _urlValidator.GetValidUrl(url);
            _logger.Info("Get correct URL.");
        }

        public void PrintStats(IEnumerable<KeyValuePair<string, int>> stats)
        {
            if (stats == null)
            {
                Console.WriteLine("An error has occurred :(");
                _logger.Info("Statistics are not printed due to errors.");
                return;
            }

            Console.WriteLine("----------------Stats----------------");

            int maxLen = stats.Max(x => x.Key.Length);

            foreach (var s in stats)
            {
                Console.WriteLine($"{WordWithIdent(s.Key, maxLen)} : {s.Value}");
            }

            Console.WriteLine("-------------------------------------");
            _logger.Info("Statistics printed.");
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
