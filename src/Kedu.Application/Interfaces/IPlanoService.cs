using Kedu.Application.DTOs;
using Kedu.Application.DTOs.Requests;

namespace Kedu.Application.Interfaces;

public interface IPlanoService
{
    Task<PlanoDto> CriarPlanoAsync(CriarPlanoRequest request);
    Task<PlanoDto?> ObterPorIdAsync(Guid id);
    Task<decimal> ObterTotalAsync(Guid id);
    Task<List<PlanoDto>> ListarPorResponsavelAsync(Guid responsavelId);
}
