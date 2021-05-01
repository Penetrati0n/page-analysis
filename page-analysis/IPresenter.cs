using System.Collections.Generic;

namespace page_analysis
{
    public interface IPresenter
    {
        void HelloMessage();
        void InputUrl(ref string url);
        void PrintStats(IEnumerable<KeyValuePair<string, int>> stats);
    }
}
