using Kedu.Domain.Entities;
using Kedu.Domain.Interfaces;
using Kedu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infrastructure.Repositories;

public class PlanoRepository : IPlanoRepository
{
    private readonly AppDbContext context;

    public PlanoRepository(AppDbContext context) => this.context = context;

    public async Task<PlanoDePagamento?> GetByIdAsync(Guid id)
        => await context.Planos
            .Include(p => p.Cobrancas)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<List<PlanoDePagamento>> GetByResponsavelIdAsync(Guid responsavelId)
        => await context.Planos
            .AsNoTracking()
            .Include(p => p.Cobrancas)
            .Where(p => p.ResponsavelId == responsavelId)
            .ToListAsync();

    public async Task<bool> ExistsByCentroDeCustoIdAsync(Guid centroDeCustoId)
        => await context.Planos.AnyAsync(p => p.CentroDeCustoId == centroDeCustoId);

    public async Task AddAsync(PlanoDePagamento plano)
    {
        context.Planos.Add(plano);
        await context.SaveChangesAsync();
    }
}
