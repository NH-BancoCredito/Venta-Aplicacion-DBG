using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Venta.Domain.Models;

namespace Venta.Infraestructure.Repositories.Base.EFConfigurations
{
    public class CategoriaEntityTypeConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categoria");
            builder.HasKey(c => c.IdCategoria);
            //builder.Property(p => p.Nombre).HasColumnName("Nombre");
        }
    }
}
