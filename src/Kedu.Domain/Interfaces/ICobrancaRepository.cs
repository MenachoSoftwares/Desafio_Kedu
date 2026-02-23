using Kedu.Domain.Entities;
using Kedu.Domain.Enums;

namespace Kedu.Domain.Interfaces;

public interface ICobrancaRepository
{
    Task<Cobranca?> GetByIdAsync(Guid id);
    Task<List<Cobranca>> GetByResponsavelIdAsync(
        Guid responsavelId,
        StatusCobranca? status = null,
        bool? vencidaApenas = null,
        DateTime? dataVencimentoDe = null,
        DateTime? dataVencimentoAte = null,
        int page = 1,
        int pageSize = 10);
    Task<int> CountByResponsavelIdAsync(
        Guid responsavelId,
        StatusCobranca? status = null,
        bool? vencidaApenas = null,
        DateTime? dataVencimentoDe = null,
        DateTime? dataVencimentoAte = null);
    Task AddAsync(Cobranca cobranca);
    Task UpdateAsync(Cobranca cobranca);
}
