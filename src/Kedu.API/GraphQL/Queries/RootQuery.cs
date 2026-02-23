using Kedu.Application.DTOs;
using Kedu.Application.Interfaces;

namespace Kedu.API.GraphQL.Queries;
public class RootQuery
{
    public async Task<PlanoDto?> GetPlano(Guid id, [Service] IPlanoService service)
        => await service.ObterPorIdAsync(id);

    public async Task<List<PlanoDto>> GetPlanosPorResponsavel(Guid responsavelId, [Service] IPlanoService service)
        => await service.ListarPorResponsavelAsync(responsavelId);

    public async Task<PaginatedResult<CobrancaDto>> GetCobrancas(
        Guid responsavelId,
        CobrancaFiltroDto? filtro = null,
        int page = 1,
        int pageSize = 10,
        [Service] ICobrancaService? service = null)
        => await service!.ListarPorResponsavelAsync(responsavelId, filtro, page, pageSize);

    public async Task<int> GetQuantidadeCobrancasPorResponsavel(
        Guid responsavelId,
        [Service] ICobrancaService service)
        => await service.ContarPorResponsavelAsync(responsavelId);

    public async Task<ResponsavelDto?> GetResponsavel(
        Guid id,
        [Service] IResponsavelService service)
        => await service.ObterPorIdAsync(id);

    public async Task<CentroDeCustoDto?> GetCentroDeCusto(
        Guid id,
        [Service] ICentroDeCustoService service)
        => await service.ObterPorIdAsync(id);

    public async Task<PaginatedResult<ResponsavelDto>> GetResponsaveis(
        int page = 1,
        int pageSize = 10,
        [Service] IResponsavelService? service = null)
        => await service!.ListarTodosAsync(page, pageSize);

    public async Task<PaginatedResult<CentroDeCustoDto>> GetCentrosDeCusto(
        int page = 1,
        int pageSize = 10,
        [Service] ICentroDeCustoService? service = null)
        => await service!.ListarTodosAsync(page, pageSize);
}
