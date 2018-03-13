using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Console;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext:DbContext
    {

        public SamuraiContext(DbContextOptions<SamuraiContext> options): base(options)
        {

        }

        //public static readonly LoggerFactory MyConsoleLoggerFactory
        //    = new LoggerFactory(new[] {
        //        new ConsoleLoggerProvider((category, level) 
        //            => category == DbLoggerCategory.Database.Command.Name 
        //            && level == LogLevel.Information, true )});

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = "Server=localhost,1431;User Id=SA;Password=P@ssw0rd;Database=DockerTestSQL;Trusted_Connection=False;";
            optionsBuilder.UseSqlServer(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new { s.SamuraiId, s.BattleId });
        }
    }
}
