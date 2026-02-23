using Kedu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ResponsavelFinanceiro> Responsaveis => Set<ResponsavelFinanceiro>();

    public DbSet<CentroDeCusto> CentrosDeCusto => Set<CentroDeCusto>();
    
    public DbSet<PlanoDePagamento> Planos => Set<PlanoDePagamento>();
    
    public DbSet<Cobranca> Cobrancas => Set<Cobranca>();
    
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
