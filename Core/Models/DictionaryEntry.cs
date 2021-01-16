using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class DictionaryEntry
    {
        [Key]
        public string Word { get; set; }
        public int Count { get; set; }
    }
}
