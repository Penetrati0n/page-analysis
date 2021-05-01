using System.ComponentModel.DataAnnotations;

namespace page_analysis
{
    public class WordValue
    {
        [Key]
        public string Word { get; set; }
        public int Value { get; set; }

        public WordValue(string word, int value)
        {
            Word = word;
            Value = value;
        }
    }
}
