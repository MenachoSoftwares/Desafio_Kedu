using Kedu.Domain.Entities;
using Kedu.Domain.Interfaces;
using Kedu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infrastructure.Repositories;

public class CentroDeCustoRepository : ICentroDeCustoRepository
{
    private readonly AppDbContext context;

    public CentroDeCustoRepository(AppDbContext context) => this.context = context;

    public async Task<CentroDeCusto?> GetByIdAsync(Guid id)
        => await context.CentrosDeCusto.FindAsync(id);

    public async Task<bool> ExistsByNomeAsync(string nome, Guid? ignorarId = null)
        => await context.CentrosDeCusto
            .AnyAsync(c => c.Nome.ToLower() == nome.ToLower() && (!ignorarId.HasValue || c.Id != ignorarId.Value));

    public async Task<List<CentroDeCusto>> GetAllAsync(int page = 1, int pageSize = 10)
        => await context.CentrosDeCusto
            .AsNoTracking()
            .OrderBy(c => c.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> CountAsync()
        => await context.CentrosDeCusto.CountAsync();

    public async Task AddAsync(CentroDeCusto centro)
    {
        context.CentrosDeCusto.Add(centro);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CentroDeCusto centro)
    {
        context.CentrosDeCusto.Update(centro);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CentroDeCusto centro)
    {
        context.CentrosDeCusto.Remove(centro);
        await context.SaveChangesAsync();
    }
}
