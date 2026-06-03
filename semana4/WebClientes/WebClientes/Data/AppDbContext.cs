using System;
using Microsoft.EntityFrameworkCore;
using WebClientes.Models;
using WebClientes.Models.Ventas;

namespace WebClientes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<TipoIdentificacionModel> TiposIdentificacion { get; set; }
        public DbSet<GeneroModel> Generos { get; set; }
        public DbSet<EstadoCivilModel> EstadosCiviles { get; set; }

        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<ProveedorModel> Proveedores { get; set; }
        public DbSet<ProductoModel> Productos { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<DetallePedidoModel> DetallesPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar PKs de tipo char como char(1) para MySQL
            modelBuilder.Entity<TipoIdentificacionModel>()
                .Property(e => e.ctipoidentificacion)
                .HasColumnType("char(1)");

            modelBuilder.Entity<GeneroModel>()
                .Property(e => e.cgenero)
                .HasColumnType("char(1)");

            modelBuilder.Entity<EstadoCivilModel>()
                .Property(e => e.cestadocivil)
                .HasColumnType("char(1)");

            // También configurar las FKs char(1) en ClienteModel
            modelBuilder.Entity<ClienteModel>()
                .Property(e => e.ctipoidentificacion)
                .HasColumnType("char(1)");

            modelBuilder.Entity<ClienteModel>()
                .Property(e => e.cgenero)
                .HasColumnType("char(1)");

            modelBuilder.Entity<ClienteModel>()
                .Property(e => e.cestadocivil)
                .HasColumnType("char(1)");

            // --- Relaciones explícitas (HasOne / WithMany / HasForeignKey) ---
            modelBuilder.Entity<ClienteModel>()
                .HasOne(c => c.TipoIdentificacion)
                .WithMany()
                .HasForeignKey(c => c.ctipoidentificacion)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClienteModel>()
                .HasOne(c => c.Genero)
                .WithMany()
                .HasForeignKey(c => c.cgenero)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClienteModel>()
                .HasOne(c => c.EstadoCivil)
                .WithMany()
                .HasForeignKey(c => c.cestadocivil)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Configuraciones Ventas ---
            modelBuilder.Entity<ProductoModel>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<PedidoModel>()
                .Property(p => p.Total)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<DetallePedidoModel>()
                .Property(d => d.PrecioUnitario)
                .HasColumnType("decimal(10,2)");

            // Relaciones Ventas
            modelBuilder.Entity<ProductoModel>()
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.cCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductoModel>()
                .HasOne(p => p.Proveedor)
                .WithMany()
                .HasForeignKey(p => p.cProveedor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PedidoModel>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ccliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetallePedidoModel>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(d => d.cPedido)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetallePedidoModel>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(d => d.cProducto)
                .OnDelete(DeleteBehavior.Restrict);

            // Datos iniciales - TiposIdentificacion
            modelBuilder.Entity<TipoIdentificacionModel>().HasData(
                new TipoIdentificacionModel { ctipoidentificacion = 'c', Descripcion = "Cédula" },
                new TipoIdentificacionModel { ctipoidentificacion = 'p', Descripcion = "Pasaporte" }
            );

            // Datos iniciales - Generos
            modelBuilder.Entity<GeneroModel>().HasData(
                new GeneroModel { cgenero = 'h', Descripcion = "Hombre" },
                new GeneroModel { cgenero = 'm', Descripcion = "Mujer" }
            );

            // Datos iniciales - EstadosCiviles
            modelBuilder.Entity<EstadoCivilModel>().HasData(
                new EstadoCivilModel { cestadocivil = 'c', Descripcion = "Casado" },
                new EstadoCivilModel { cestadocivil = 's', Descripcion = "Soltero" },
                new EstadoCivilModel { cestadocivil = 'v', Descripcion = "Viudo" },
                new EstadoCivilModel { cestadocivil = 'd', Descripcion = "Divorciado" }
            );

            // Seed Data Ventas
            modelBuilder.Entity<CategoriaModel>().HasData(
                new CategoriaModel { cCategoria = 1, Nombre = "Electrónica", Descripcion = "Equipos electrónicos", Estado = true, FechaCreacion = new DateTime(2025, 1, 1) },
                new CategoriaModel { cCategoria = 2, Nombre = "Hogar", Descripcion = "Artículos para el hogar", Estado = true, FechaCreacion = new DateTime(2025, 1, 1) }
            );

            modelBuilder.Entity<ProveedorModel>().HasData(
                new ProveedorModel { cProveedor = 1, RazonSocial = "Sony Ecuador", Ruc = "1790000000001", Telefono = "022222222", Correo = "info@sony.ec" },
                new ProveedorModel { cProveedor = 2, RazonSocial = "Samsung Ecuador", Ruc = "1790000000002", Telefono = "022222223", Correo = "info@samsung.ec" }
            );

            modelBuilder.Entity<ProductoModel>().HasData(
                new ProductoModel { cProducto = 1, Nombre = "Televisor 55'", Precio = 500.50m, Stock = 10, cCategoria = 1, cProveedor = 2 },
                new ProductoModel { cProducto = 2, Nombre = "PlayStation 5", Precio = 800.00m, Stock = 5, cCategoria = 1, cProveedor = 1 },
                new ProductoModel { cProducto = 3, Nombre = "Licuadora", Precio = 50.00m, Stock = 20, cCategoria = 2, cProveedor = 2 }
            );
        }
    }
}
