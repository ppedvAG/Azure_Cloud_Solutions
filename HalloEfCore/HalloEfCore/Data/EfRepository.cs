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
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HalloEf;Trusted_Connection=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Mitarbeiter>().ToTable("Mitarbeiter");
            modelBuilder.Entity<Kunde>().ToTable("Kunden");
        }
    }
}
