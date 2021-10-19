using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RNC_API.Modelo
{
    public partial class companias_dbContext : DbContext
    {
        public companias_dbContext()
        {
        }

        public companias_dbContext(DbContextOptions<companias_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contribuyente> Contribuyentes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-RJ7RIQK\\SQLEXPRESS;Database=companias_db;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Contribuyente>(entity =>
            {
                entity.ToTable("Contribuyente");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NombreComercial)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RazonSocial)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Rnc)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
