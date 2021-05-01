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
        }
        
        public IEnumerable<KeyValuePair<string, int>> GetStatistic(string urlAdress, bool writeToDataBase = false)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            DownloadPage(urlAdress);

            // Если файл со страницей не найден, то произошла какая-то ошибка.
            if (!File.Exists(pagePath))
                return null;

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

            return result;
        }

        private static Encoding GetEncodingFromPage(string urlAdress)
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
                // Log
                return null;
            }

            return charSet == null || charSet == "windows-1251" ? Encoding.Default : Encoding.GetEncoding(charSet);
        }

        private void DownloadPage(string urlAdress)
        {
            Encoding encoding = GetEncodingFromPage(urlAdress);

            if (encoding == null) return;

            WebClient client = new() { Encoding = encoding };

            try
            {
                client.DownloadFile(urlAdress, pagePath);
            }
            catch (Exception ex)
            {
                // Log
            }
        }

    }
}
