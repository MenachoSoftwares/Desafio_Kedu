using Kedu.Application.DTOs;
using Kedu.Application.DTOs.Requests;

namespace Kedu.Application.Interfaces;

public interface ICentroDeCustoService
{
    Task<CentroDeCustoDto> CriarAsync(CriarCentroDeCustoRequest request);
    Task<CentroDeCustoDto?> ObterPorIdAsync(Guid id);
    Task<PaginatedResult<CentroDeCustoDto>> ListarTodosAsync(int page = 1, int pageSize = 10);
    Task<CentroDeCustoDto> AtualizarAsync(Guid id, CriarCentroDeCustoRequest request);
    Task DeletarAsync(Guid id);
}
