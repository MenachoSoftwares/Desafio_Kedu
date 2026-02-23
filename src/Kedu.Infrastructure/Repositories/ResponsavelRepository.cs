using Kedu.Domain.Entities;
using Kedu.Domain.Interfaces;
using Kedu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infrastructure.Repositories;

public class ResponsavelRepository : IResponsavelRepository
{
    private readonly AppDbContext context;

    public ResponsavelRepository(AppDbContext context) => this.context = context;

    public async Task<ResponsavelFinanceiro?> GetByIdAsync(Guid id)
        => await context.Responsaveis.FindAsync(id);

    public async Task<List<ResponsavelFinanceiro>> GetAllAsync(int page = 1, int pageSize = 10)
        => await context.Responsaveis
            .AsNoTracking()
            .OrderBy(r => r.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> CountAsync()
        => await context.Responsaveis.CountAsync();

    public async Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj)
        => await context.Responsaveis.AnyAsync(r => r.CpfCnpj == cpfCnpj);

    public async Task AddAsync(ResponsavelFinanceiro responsavel)
    {
        context.Responsaveis.Add(responsavel);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ResponsavelFinanceiro responsavel)
    {
        context.Responsaveis.Update(responsavel);
        await context.SaveChangesAsync();
    }
}
