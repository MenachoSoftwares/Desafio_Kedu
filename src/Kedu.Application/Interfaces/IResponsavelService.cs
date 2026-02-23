using Kedu.Application.DTOs;
using Kedu.Application.DTOs.Requests;

namespace Kedu.Application.Interfaces;

public interface IResponsavelService
{
    Task<ResponsavelDto> CriarAsync(CriarResponsavelRequest request);
    Task<ResponsavelDto?> ObterPorIdAsync(Guid id);
    Task<PaginatedResult<ResponsavelDto>> ListarTodosAsync(int page = 1, int pageSize = 10);
    Task<ResponsavelDto> AtualizarAsync(Guid id, AtualizarResponsavelRequest request);
}
