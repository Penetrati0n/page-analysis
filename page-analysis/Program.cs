using System;
using System.Text;

namespace page_analysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            PageInfo info = new PageInfo();         // Класс, который анализирует страницу.
            IPresenter presenter = new Presenter(); // Класс, печатающий информацию в консоль.
            string url = string.Empty;

            presenter.HelloMessage();               // Печатаем приветственное сообщение
            presenter.InputUrl(ref url);            // Ввод URL

            var data = info.GetStatistic(url);      // Получаем статистику
            presenter.PrintStats(data);             // Печатаем статистику
        }
    }
}
