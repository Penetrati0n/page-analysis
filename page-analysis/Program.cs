using System;

namespace page_analysis
{
    class Program
    {
        static void Main(string[] args)
        {
            PageInfo pi = new PageInfo();
            pi.DownloadPage(Console.ReadLine());
        }
    }
}
