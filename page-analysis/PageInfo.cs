using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace page_analysis
{
    class PageInfo
    {
        private readonly string _urlPattern;
        private readonly Regex _urlRegex;
        private readonly string _pagePath;

        public PageInfo()
        {
            _urlPattern = "((https?:\\/\\/)|)(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b";
            _urlRegex = new Regex(_urlPattern);
            _pagePath = "page.html";
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
                // Log
            }

            return charSet == null ? Encoding.Default : Encoding.GetEncoding(charSet);
        }

        public void DownloadPage(string urlAdress)
        {
            Encoding encoding = GetEncodingFromPage(urlAdress);

            WebClient client = new WebClient() { Encoding = encoding };

            try
            {
                client.DownloadFile(urlAdress, _pagePath);
            }
            catch (Exception ex)
            {
                // Log
            }
        }
    }
}
