using Kedu.Application.DTOs;
using Kedu.Application.DTOs.Requests;
using Kedu.Application.Interfaces;

namespace Kedu.API.GraphQL.Mutations;
public class RootMutation
{
    public async Task<PlanoDto> CriarPlano(CriarPlanoRequest input, [Service] IPlanoService service)
        => await service.CriarPlanoAsync(input);

    public async Task<CobrancaDto> RegistrarPagamento(Guid cobrancaId, [Service] ICobrancaService service)
        => await service.RegistrarPagamentoAsync(cobrancaId);

    public async Task<ResponsavelDto> CriarResponsavel(CriarResponsavelRequest input, [Service] IResponsavelService service)
        => await service.CriarAsync(input);

    public async Task<CentroDeCustoDto> CriarCentroDeCusto(CriarCentroDeCustoRequest input, [Service] ICentroDeCustoService service)
        => await service.CriarAsync(input);

    public async Task<ResponsavelDto> AtualizarResponsavel(Guid id, AtualizarResponsavelRequest input, [Service] IResponsavelService service)
        => await service.AtualizarAsync(id, input);

    public async Task<CentroDeCustoDto> AtualizarCentroDeCusto(Guid id, CriarCentroDeCustoRequest input, [Service] ICentroDeCustoService service)
        => await service.AtualizarAsync(id, input);

    public async Task<bool> DeletarCentroDeCusto(Guid id, [Service] ICentroDeCustoService service)
    {
        await service.DeletarAsync(id);
        return true;
    }

    public async Task<CobrancaDto> CancelarCobranca(Guid cobrancaId, [Service] ICobrancaService service)
        => await service.CancelarCobrancaAsync(cobrancaId);

    public async Task<CobrancaDto> AlterarVencimentoCobranca(Guid cobrancaId, AlterarVencimentoRequest input, [Service] ICobrancaService service)
        => await service.AlterarVencimentoAsync(cobrancaId, input.NovaDataVencimento);

    public async Task<CobrancaDto> AlterarMetodoPagamento(Guid cobrancaId, AlterarMetodoPagamentoRequest input, [Service] ICobrancaService service)
        => await service.AlterarMetodoPagamentoAsync(cobrancaId, input.NovoMetodoPagamento);
}
