using crud_maui.Models;
using crud_maui.Ultilidade;
using Microsoft.EntityFrameworkCore;

// Mudar Empregado pra Colaborador;

namespace crud_maui.DataAcess
{
    public class EmpregadoDbContext : DbContext
    {
        public DbSet<Empregado> Empregados { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conectionDB = $"Filename={ConnectDB.DevolverRota("empregados.db")}";
            optionsBuilder.UseSqlite(conectionDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empregado>(entity =>
            {
                entity.HasKey(col => col.IdEmpregado);
                entity.Property(col => col.IdEmpregado).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
