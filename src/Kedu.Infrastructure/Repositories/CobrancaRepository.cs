using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Domain.Interfaces;
using Kedu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infrastructure.Repositories;

public class CobrancaRepository : ICobrancaRepository
{
    private readonly AppDbContext context;

    public CobrancaRepository(AppDbContext context) => this.context = context;

    public async Task<Cobranca?> GetByIdAsync(Guid id)
        => await context.Cobrancas.FindAsync(id);

    public async Task<List<Cobranca>> GetByResponsavelIdAsync(
        Guid responsavelId,
        StatusCobranca? status = null,
        bool? vencidaApenas = null,
        DateTime? dataVencimentoDe = null,
        DateTime? dataVencimentoAte = null,
        int page = 1,
        int pageSize = 10)
    {
        var query = BuildQuery(responsavelId, status, vencidaApenas, dataVencimentoDe, dataVencimentoAte);

        return await query
            .OrderBy(c => c.DataVencimento)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountByResponsavelIdAsync(
        Guid responsavelId,
        StatusCobranca? status = null,
        bool? vencidaApenas = null,
        DateTime? dataVencimentoDe = null,
        DateTime? dataVencimentoAte = null)
    {
        var query = BuildQuery(responsavelId, status, vencidaApenas, dataVencimentoDe, dataVencimentoAte);
        return await query.CountAsync();
    }

    private IQueryable<Cobranca> BuildQuery(
        Guid responsavelId,
        StatusCobranca? status,
        bool? vencidaApenas,
        DateTime? dataVencimentoDe,
        DateTime? dataVencimentoAte)
    {
        var query = context.Cobrancas
            .AsNoTracking()
            .Join(
                context.Planos,
                c => c.PlanoId,
                p => p.Id,
                (c, p) => new { Cobranca = c, Plano = p })
            .Where(x => x.Plano.ResponsavelId == responsavelId)
            .Select(x => x.Cobranca);

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        if (vencidaApenas == true)
        {
            var agora = DateTime.UtcNow;
            query = query.Where(c =>
                c.Status != StatusCobranca.PAGA &&
                c.Status != StatusCobranca.CANCELADA &&
                c.DataVencimento < agora);
        }

        if (dataVencimentoDe.HasValue)
            query = query.Where(c => c.DataVencimento >= dataVencimentoDe.Value);

        if (dataVencimentoAte.HasValue)
            query = query.Where(c => c.DataVencimento <= dataVencimentoAte.Value);

        return query;
    }

    public async Task AddAsync(Cobranca cobranca)
    {
        context.Cobrancas.Add(cobranca);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cobranca cobranca)
    {
        context.Cobrancas.Update(cobranca);
        await context.SaveChangesAsync();
    }
}
