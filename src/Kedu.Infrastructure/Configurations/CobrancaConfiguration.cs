using Kedu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kedu.Infrastructure.Configurations;

public class CobrancaConfiguration : IEntityTypeConfiguration<Cobranca>
{
    public void Configure(EntityTypeBuilder<Cobranca> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Valor).HasPrecision(18, 2).IsRequired();
        builder.Property(c => c.MetodoPagamento).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.CodigoPagamento).HasMaxLength(200);
        builder.Ignore(c => c.EstaVencida);
    }
}
