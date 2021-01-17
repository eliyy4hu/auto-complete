using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class DataContext : DbContext
    {
        public DbSet<DictionaryEntry> FrequencyDictionary { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-29MERC1;Database=DictionaryDB;Trusted_Connection=True;");
        }
    }
}
