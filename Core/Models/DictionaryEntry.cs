using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class DictionaryEntry
    {
        [Key]
        public string Word { get; set; }
        public int Count { get; set; }
    }
}
