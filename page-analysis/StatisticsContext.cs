using Microsoft.EntityFrameworkCore;

namespace page_analysis
{
    public class StatisticsContext : DbContext
    {
        public DbSet<WordValue> Statistics { get; set; }

        public StatisticsContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=statisticsWordsDB;Username=<username>;Password=<password>");
        }
    }
}
