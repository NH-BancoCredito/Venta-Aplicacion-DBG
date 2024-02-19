
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Venta.Domain.Models;

namespace Venta.Infraestructure.Repositories.Base.EFConfigurations
{
    public class ProductoEntityTypeConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Producto");
            builder.HasKey(c => c.IdProducto);
            //builder.Property(c => c.PrecioUnitario).HasPrecision(2);
            builder.HasOne(p => p.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(p => p.IdCategoria);
        }
    }
}
