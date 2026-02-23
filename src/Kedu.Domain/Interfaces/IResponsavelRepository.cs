using Kedu.Domain.Entities;

namespace Kedu.Domain.Interfaces;

public interface IResponsavelRepository
{
    Task<ResponsavelFinanceiro?> GetByIdAsync(Guid id);
    Task<List<ResponsavelFinanceiro>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<int> CountAsync();
    Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj);
    Task AddAsync(ResponsavelFinanceiro responsavel);
    Task UpdateAsync(ResponsavelFinanceiro responsavel);
}
