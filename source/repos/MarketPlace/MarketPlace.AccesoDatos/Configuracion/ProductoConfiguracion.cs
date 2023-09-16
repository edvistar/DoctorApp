using MarketPlace.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Configuracion
{
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.Property(x=> x.Id).IsRequired();
            builder.Property(x=> x.Name).IsRequired().HasMaxLength(60);
            builder.Property(x=> x.Description).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Offer).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Cost).IsRequired();
            builder.Property(x => x.CategoriaId).IsRequired();
            builder.Property(x => x.MarcaId).IsRequired();
            builder.Property(x => x.ImageUrl).IsRequired(false);
            builder.Property(x => x.PadreId).IsRequired(false);

            /*Relations*/

            builder.HasOne(x => x.Categoria).WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Marca).WithMany()
                .HasForeignKey(X => X.MarcaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Padre).WithMany()
                .HasForeignKey(x => x.PadreId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
