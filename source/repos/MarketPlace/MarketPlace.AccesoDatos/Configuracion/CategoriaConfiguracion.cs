using MarketPlace.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class CategoriaConfiguracion : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.Property(x=> x.Id).IsRequired();
            builder.Property(x=> x.Name).IsRequired().HasMaxLength(60);
            builder.Property(x=> x.Description).IsRequired().HasMaxLength(100);
            builder.Property(x=> x.Status).IsRequired();
        }
    }
}
