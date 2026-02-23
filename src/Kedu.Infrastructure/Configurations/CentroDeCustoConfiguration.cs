using Kedu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kedu.Infrastructure.Configurations;

public class CentroDeCustoConfiguration : IEntityTypeConfiguration<CentroDeCusto>
{
    public void Configure(EntityTypeBuilder<CentroDeCusto> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Descricao).HasMaxLength(500);
    }
}
