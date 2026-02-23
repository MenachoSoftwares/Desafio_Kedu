using Kedu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kedu.Infrastructure.Configurations;

public class ResponsavelConfiguration : IEntityTypeConfiguration<ResponsavelFinanceiro>
{
    public void Configure(EntityTypeBuilder<ResponsavelFinanceiro> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Nome).HasMaxLength(200).IsRequired();
        builder.Property(r => r.Email).HasMaxLength(200).IsRequired();
        builder.Property(r => r.Telefone).HasMaxLength(20);
        builder.Property(r => r.CpfCnpj).HasMaxLength(18).IsRequired();
    }
}
