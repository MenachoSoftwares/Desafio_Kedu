using Kedu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kedu.Infrastructure.Configurations;

public class PlanoConfiguration : IEntityTypeConfiguration<PlanoDePagamento>
{
    public void Configure(EntityTypeBuilder<PlanoDePagamento> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasMany(p => p.Cobrancas)
            .WithOne()
            .HasForeignKey(c => c.PlanoId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(p => p.ValorTotal);
    }
}
