using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Infraestructura.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base (options)
        {
            
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet <Subasta> Subastas { get; set; }
        public DbSet<Puja> Pujas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.FechaRegistro).IsRequired();

            });

            modelBuilder.Entity<Categoria>(entity => 
            {
                entity.ToTable("Categorias");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c=>c.UrlIcono).HasMaxLength(500);                                                                         
            });

            modelBuilder.Entity<Subasta>(entity => 
            {
                entity.ToTable("Subastas");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).ValueGeneratedOnAdd();
                entity.Property(s => s.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Descripcion).IsRequired().HasMaxLength(2000);
                entity.Property(s => s.UrlImagen).HasMaxLength(500);
                entity.Property(s => s.PrecioBase).HasPrecision(18, 2).IsRequired();
                entity.Property(s => s.IncrementoMinimo).HasPrecision(18, 2).IsRequired();
                entity.Property(s => s.FechaIncio).IsRequired();
                entity.Property(s => s.FechaFin).IsRequired();
                entity.Property(s => s.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();
                entity.Property(s => s.Version).IsConcurrencyToken();

                entity.HasOne(s => s.Vendedor)
                .WithMany(c => c.Subastas)
                .HasForeignKey(s => s.VendedorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Categoria)
                .WithMany(c=>c.Subastas)
                .HasForeignKey(s=>s.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Puja>(entity =>
            {
                entity.ToTable("Pujas");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Monto).HasPrecision(18, 2).IsRequired();
                entity.Property(p => p.FechaPuja).IsRequired();
              
                entity.HasOne(p => p.Subasta)
                    .WithMany(s => s.Pujas)
                    .HasForeignKey(p => p.SubastaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Comprador)
                    .WithMany(u => u.Pujas)
                    .HasForeignKey(p => p.CompradorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }


    }
}
