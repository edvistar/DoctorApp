using MarketPlace.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace MarketPlace.AccesoDatos
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Bodega> Bodega { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Marca> Marca { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<UsuarioAplicacion> UsuarioAplicacion { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<UsuarioAplicacion>()
                .HasMany(u => u.Producto)
                .WithOne(p => p.UsuarioAplicacion)
                .HasForeignKey(p => p.UsuarioAplicacionId);
            builder.Entity<UsuarioAplicacion>()
                .HasMany(u => u.Categoria)
                .WithOne(p => p.UsuarioAplicacion)
                .HasForeignKey(p => p.UsuarioAplicacionId);
            builder.Entity<UsuarioAplicacion>()
                .HasMany(u => u.Marca)
                .WithOne(p => p.UsuarioAplicacion)
                .HasForeignKey(p => p.UsuarioAplicacionId);
            builder.Entity<UsuarioAplicacion>()
                .HasMany(u => u.Bodega)
                .WithOne(p => p.UsuarioAplicacion)
                .HasForeignKey(p => p.UsuarioAplicacionId);

        }
        
    }
}