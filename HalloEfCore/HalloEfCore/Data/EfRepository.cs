using HalloEfCore.Model;
using Microsoft.EntityFrameworkCore;

namespace HalloEfCore.Data
{
    internal class EfRepository : DbContext
    {
        public DbSet<Kunde> Kunden { get; set; }
        public DbSet<Mitarbeiter> Mitarbeiter { get; set; }
        public DbSet<Abteilung> Abteilungen { get; set; }


        public EfRepository()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HalloEf;Trusted_Connection=true;");
#else
            optionsBuilder.UseSqlServer("Server=tcp:freds-server-andre.database.windows.net,1433;Initial Catalog=FredsDB;Persist Security Info=False;User ID=Fred;Password=TYuxqka4;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Mitarbeiter>().ToTable("Mitarbeiter");
            modelBuilder.Entity<Kunde>().ToTable("Kunden");
        }
    }
}
