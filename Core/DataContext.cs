using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Core
{
    public class DataContext : DbContext
    {
        public DbSet<DictionaryEntry> FrequencyDictionary { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conString = ConfigurationManager.ConnectionStrings["dictionaryDb"];
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["dictionaryDb"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}