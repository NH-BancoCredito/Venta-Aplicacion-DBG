using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Venta.Domain.Models;

namespace Venta.Infraestructure.Repositories.Base.EFConfigurations
{
    public class VentaEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Models.Venta>, IEntityTypeConfiguration<VentaDetalle>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Models.Venta> builder)
        {
            builder.ToTable("Venta");
            builder.HasKey(c => c.IdVenta);
            builder.HasOne(p => p.Cliente).WithMany(p => p.Ventas)
                .HasForeignKey(p => p.IdCliente);
        }

        public void Configure(EntityTypeBuilder<VentaDetalle> builder)
        {
            builder.ToTable("VentaDetalle");
            builder.HasKey(c => c.IdVentaDetalle);
            
            builder.HasOne(p => p.Venta).WithMany(p => p.Detalle)
                .HasForeignKey(p => p.IdVenta);
            
            builder.HasOne(p => p.Producto).WithMany(p => p.VentaDetalle)
                .HasForeignKey(p => p.IdProducto);
        }
    }
}
