using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace page_analysis
{
    class PageInfo
    {
        private int pageCount;          // Номер страницы.
        private string pagePath;        // Путь к файлу со страницей.
        private readonly Filter _filter;// Фильтр, убирающий лишнюю информацию из страницы и возвращающий слова.
        private readonly Logger _logger;

        private int GetPageCount()
        {
            return pageCount;
        }
        private void SetPageCount(int value)
        {
            pageCount = value;
            pagePath = $"page{value}.html";
        }

        public PageInfo()
        {
            SetPageCount(1);
            while (File.Exists(pagePath))
                SetPageCount(GetPageCount() + 1);

            _filter = new Filter();
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        public IEnumerable<KeyValuePair<string, int>> GetStatistic(string urlAdress, bool writeToDataBase = false)
        {
            Dictionary<string, int> result = new();
            DownloadPage(urlAdress);

            // Если файл со страницей не найден, то произошла какая-то ошибка.
            if (!File.Exists(pagePath))
            {
                _logger.Error("File is not exists.");
                return null;
            }

            string page = File.ReadAllText(pagePath);

            // Получаем слова из страницы.
            var words = _filter.GetWords(page);
            
            // Собираем статистику по словам.
            foreach (var w in words)
            {
                if (result.ContainsKey(w))
                    result[w]++;
                else
                    result.Add(w, 1);
            }

            // Сортируем результат.
            result = result.OrderBy(k => k.Value).ToDictionary(x => x.Key, x => x.Value);

            // Записываем результат в базу данных.
            if (writeToDataBase)
            {
                try
                {
                    using (var context = new StatisticsContext())
                    {
                        foreach (var r in context.Statistics)
                        {
                            context.Statistics.Remove(r);
                        }

                        foreach (var r in result)
                        {
                            context.Statistics.Add(new WordValue(r.Key, r.Value));
                        }

                        context.SaveChanges();
                    }
                    _logger.Info("Statistics recorded in the database.");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error while writing statistics to the database.");
                }
            }

            _logger.Info("Statistics received.");
            return result;
        }

        private Encoding GetEncodingFromPage(string urlAdress)
        {
            string charSet = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAdress);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    charSet = response.CharacterSet;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Encoding not received.");
                return null;
            }

            _logger.Info("Return " + charSet + ".");
            return charSet == null || charSet == "windows-1251" ? Encoding.Default : Encoding.GetEncoding(charSet);
        }

        private void DownloadPage(string urlAdress)
        {
            Encoding encoding = GetEncodingFromPage(urlAdress);

            if (encoding == null)
            {
                _logger.Info("Page not downloaded.");
                return;
            }

            WebClient client = new() { Encoding = encoding };

            try
            {
                client.DownloadFile(urlAdress, pagePath);
                _logger.Info("Page downloaded.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Page not downloaded.");
            }
        }

    }
}
