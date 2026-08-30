using Dominio.Entities;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        public DbSet <Billetera> Billeteras { get; set; }
        public DbSet<AuditoriaLog> AuditoriaLogs {  get; set; }
        public DbSet< TransaccionLedger> TransaccionLedgers { get; set; }
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
                entity.Property(s => s.FechaInicio).IsRequired();
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

            modelBuilder.Entity<Billetera>(entity =>
            {

                entity.ToTable("Billeteras");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).ValueGeneratedOnAdd();
                entity.Property(b => b.SaldoTotal).HasPrecision(18, 2).IsRequired();
                entity.Property(b => b.SaldoRetenido).HasPrecision(18, 2).IsRequired();
                entity.Ignore(b=> b.SaldoDisponible);
                entity.Property(b => b.Version).IsConcurrencyToken();

                entity.HasOne(b => b.Usuario)
                .WithOne(u => u.Billetera)
                .HasForeignKey<Billetera>(b => b.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(b => b.UsuarioId).IsUnique();

            });

            modelBuilder.Entity<AuditoriaLog>(entity =>
            {
                entity.ToTable("AuditoriaLogs");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).ValueGeneratedOnAdd();
                entity.Property(a => a.Entidad).IsRequired().HasMaxLength(50);
                entity.Property(a => a.Accion).IsRequired().HasMaxLength(100);
                entity.Property(a => a.DetalleJson).HasColumnType("nvarchar(max)");
                entity.Property(a => a.Fecha).IsRequired();

                entity.HasOne(a => a.Usuario)
                    .WithMany(u => u.AuditoriaLogs)
                    .HasForeignKey(a => a.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TransaccionLedger>(entity =>
            {
                entity.ToTable("TransaccionesLedger");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                entity.Property(t => t.Tipo).HasConversion<string>().HasMaxLength(30).IsRequired();
                entity.Property(t => t.Monto).HasPrecision(18, 2).IsRequired();
                entity.Property(t => t.Fecha).IsRequired();

                entity.HasOne(t => t.Billetera)
                    .WithMany(b => b.Transacciones)
                    .HasForeignKey(t => t.BilleteraId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Subasta)
                    .WithMany(s => s.Transacciones)
                    .HasForeignKey(t => t.SubastaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================
            // SEED DATA
            // =========================

            var fechaBase = new DateTime(2026, 8, 30, 12, 0, 0);

            // 1. Usuarios
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Email = "vendedor@test.com", Nombre = "Vendedor", PasswordHash = "hash123", FechaRegistro = fechaBase.AddDays(-10) },
                new Usuario { Id = 2, Email = "comprador1@test.com", Nombre = "Comprador 1", PasswordHash = "hash123", FechaRegistro = fechaBase.AddDays(-5) },
                new Usuario { Id = 3, Email = "comprador2@test.com", Nombre = "Comprador 2", PasswordHash = "hash123", FechaRegistro = fechaBase.AddDays(-2) },
                new Usuario { Id = 4, Email = "sinfondos@test.com", Nombre = "Sin Fondos", PasswordHash = "hash123", FechaRegistro = fechaBase.AddDays(-1) }
            );

            // 2. Billeteras
            modelBuilder.Entity<Billetera>().HasData(
                new Billetera { Id = 1, UsuarioId = 1, SaldoTotal = 0m, SaldoRetenido = 0m, Version = 1 },
                new Billetera { Id = 2, UsuarioId = 2, SaldoTotal = 150000m, SaldoRetenido = 45000m, Version = 1 },
                new Billetera { Id = 3, UsuarioId = 3, SaldoTotal = 200000m, SaldoRetenido = 0m, Version = 1 },
                new Billetera { Id = 4, UsuarioId = 4, SaldoTotal = 500m, SaldoRetenido = 0m, Version = 1 }
            );

            // 3. Categorías
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Tecnología", UrlIcono = "tech.png" },
                new Categoria { Id = 2, Nombre = "Coleccionables", UrlIcono = "col.png" },
                new Categoria { Id = 3, Nombre = "Indumentaria", UrlIcono = "ropa.png" },
                new Categoria { Id = 4, Nombre = "Vehículos", UrlIcono = "auto.png" }
            );

            // 4. Subastas
            modelBuilder.Entity<Subasta>().HasData(
                new Subasta
                {
                    Id = 1,
                    VendedorId = 1,
                    CategoriaId = 1,
                    Titulo = "Notebook Pro",
                    Descripcion = "Subasta activa estándar",
                    UrlImagen = "img1.png",
                    PrecioBase = 30000m,
                    IncrementoMinimo = 1000m,
                    FechaInicio = fechaBase.AddHours(-1),
                    FechaFin = fechaBase.AddMinutes(30),
                    Estado = EstadoSubasta.Activa,
                    Version = 1
                },

                new Subasta
                {
                    Id = 2,
                    VendedorId = 1,
                    CategoriaId = 2,
                    Titulo = "Reloj Antiguo",
                    Descripcion = "Subasta crítica para probar anti-sniping",
                    UrlImagen = "img2.png",
                    PrecioBase = 10000m,
                    IncrementoMinimo = 500m,
                    FechaInicio = fechaBase.AddHours(-2),
                    FechaFin = fechaBase.AddMinutes(1),
                    Estado = EstadoSubasta.Activa,
                    Version = 1
                },

                new Subasta
                {
                    Id = 3,
                    VendedorId = 1,
                    CategoriaId = 4,
                    Titulo = "Auto Usado",
                    Descripcion = "Subasta programada",
                    UrlImagen = "img3.png",
                    PrecioBase = 1500000m,
                    IncrementoMinimo = 50000m,
                    FechaInicio = fechaBase.AddHours(24),
                    FechaFin = fechaBase.AddHours(48),
                    Estado = EstadoSubasta.Programada,
                    Version = 1
                },

                new Subasta
                {
                    Id = 4,
                    VendedorId = 1,
                    CategoriaId = 1,
                    Titulo = "Monitor 24",
                    Descripcion = "Subasta vencida con ganador",
                    UrlImagen = "img4.png",
                    PrecioBase = 20000m,
                    IncrementoMinimo = 1000m,
                    FechaInicio = fechaBase.AddDays(-3),
                    FechaFin = fechaBase.AddDays(-1),
                    Estado = EstadoSubasta.Activa,
                    Version = 1
                },

                new Subasta
                {
                    Id = 5,
                    VendedorId = 1,
                    CategoriaId = 3,
                    Titulo = "Campera Cuero",
                    Descripcion = "Subasta vencida sin ofertas",
                    UrlImagen = "img5.png",
                    PrecioBase = 50000m,
                    IncrementoMinimo = 2000m,
                    FechaInicio = fechaBase.AddDays(-5),
                    FechaFin = fechaBase.AddDays(-2),
                    Estado = EstadoSubasta.Activa,
                    Version = 1
                }
            );

            // 5. Pujas
            modelBuilder.Entity<Puja>().HasData(
                new Puja { Id = 1, SubastaId = 1, CompradorId = 3, Monto = 35000m, FechaPuja = fechaBase.AddMinutes(-40) },
                new Puja { Id = 2, SubastaId = 1, CompradorId = 2, Monto = 45000m, FechaPuja = fechaBase.AddMinutes(-20) },
                new Puja { Id = 3, SubastaId = 4, CompradorId = 3, Monto = 25000m, FechaPuja = fechaBase.AddDays(-2) }
            );

            // 6. Ledger
            modelBuilder.Entity<TransaccionLedger>().HasData(
                new TransaccionLedger
                {
                    Id = 1,
                    BilleteraId = 2,
                    Tipo = TipoTransaccion.Deposito,
                    Monto = 150000m,
                    Fecha = fechaBase.AddDays(-4),
                    SubastaId = null
                },

                new TransaccionLedger
                {
                    Id = 2,
                    BilleteraId = 2,
                    Tipo = TipoTransaccion.Retencion,
                    Monto = 45000m,
                    Fecha = fechaBase.AddMinutes(-20),
                    SubastaId = 1
                }
            );













        }





    }
}
