using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class LastUpdate
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}
