using Kedu.Domain.Entities;

namespace Kedu.Domain.Interfaces;

public interface IPlanoRepository
{
    Task<PlanoDePagamento?> GetByIdAsync(Guid id);
    Task<List<PlanoDePagamento>> GetByResponsavelIdAsync(Guid responsavelId);
    Task<bool> ExistsByCentroDeCustoIdAsync(Guid centroDeCustoId);
    Task AddAsync(PlanoDePagamento plano);
}
