using Kedu.Application.DTOs;
using Kedu.Application.DTOs.Requests;
using Kedu.Domain.Enums;

namespace Kedu.Application.Interfaces;

public interface ICobrancaService
{
    Task<CobrancaDto> RegistrarPagamentoAsync(Guid cobrancaId);
    Task<CobrancaDto> CancelarCobrancaAsync(Guid cobrancaId);
    Task<CobrancaDto> AlterarVencimentoAsync(Guid cobrancaId, DateTime novaDataVencimento);
    Task<CobrancaDto> AlterarMetodoPagamentoAsync(Guid cobrancaId, MetodoPagamento novoMetodo);
    Task<PaginatedResult<CobrancaDto>> ListarPorResponsavelAsync(
        Guid responsavelId, CobrancaFiltroDto? filtro = null, int page = 1, int pageSize = 10);
    Task<int> ContarPorResponsavelAsync(Guid responsavelId);
}
