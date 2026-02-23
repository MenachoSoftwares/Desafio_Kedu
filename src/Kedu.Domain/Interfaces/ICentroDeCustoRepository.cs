using Kedu.Domain.Entities;

namespace Kedu.Domain.Interfaces;

public interface ICentroDeCustoRepository
{
    Task<CentroDeCusto?> GetByIdAsync(Guid id);
    Task<bool> ExistsByNomeAsync(string nome, Guid? ignorarId = null);
    Task<List<CentroDeCusto>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<int> CountAsync();
    Task AddAsync(CentroDeCusto centro);
    Task UpdateAsync(CentroDeCusto centro);
    Task DeleteAsync(CentroDeCusto centro);
}
