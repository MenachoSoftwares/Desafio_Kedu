using Kedu.Application.DTOs;
using Kedu.Application.Interfaces;
using Kedu.Domain.Enums;
using Kedu.Domain.Exceptions;
using Kedu.Domain.Interfaces;

namespace Kedu.Application.Services;

public class CobrancaService : ICobrancaService
{
    private readonly ICobrancaRepository cobrancaRepo;

    public CobrancaService(ICobrancaRepository cobrancaRepo) => this.cobrancaRepo = cobrancaRepo;
    public async Task<CobrancaDto> RegistrarPagamentoAsync(Guid cobrancaId)
    {
        var cobranca = await cobrancaRepo.GetByIdAsync(cobrancaId)
            ?? throw new NotFoundException("Cobrança não encontrada.");

        if (cobranca.Status == StatusCobranca.CANCELADA)
            throw new DomainException("Não é possível pagar uma cobrança cancelada.");

        cobranca.RegistrarPagamento();

        await cobrancaRepo.UpdateAsync(cobranca);
        return CobrancaDto.FromEntity(cobranca);
    }

    public async Task<CobrancaDto> CancelarCobrancaAsync(Guid cobrancaId)
    {
        var cobranca = await cobrancaRepo.GetByIdAsync(cobrancaId)
            ?? throw new NotFoundException("Cobrança não encontrada.");

        if (cobranca.Status == StatusCobranca.PAGA)
            throw new DomainException("Não é possível cancelar uma cobrança já paga.");

        cobranca.Cancelar();

        await cobrancaRepo.UpdateAsync(cobranca);
        return CobrancaDto.FromEntity(cobranca);
    }

    public async Task<CobrancaDto> AlterarVencimentoAsync(Guid cobrancaId, DateTime novaDataVencimento)
    {
        var cobranca = await cobrancaRepo.GetByIdAsync(cobrancaId)
            ?? throw new NotFoundException("Cobrança não encontrada.");

        if (cobranca.Status != StatusCobranca.PENDENTE)
            throw new DomainException("Só é possível alterar o vencimento de cobranças pendentes.");

        if (novaDataVencimento <= DateTime.UtcNow)
            throw new DomainException("A nova data de vencimento deve ser no futuro.");

        cobranca.AlterarVencimento(novaDataVencimento);

        await cobrancaRepo.UpdateAsync(cobranca);
        return CobrancaDto.FromEntity(cobranca);
    }

    public async Task<CobrancaDto> AlterarMetodoPagamentoAsync(Guid cobrancaId, MetodoPagamento novoMetodo)
    {
        var cobranca = await cobrancaRepo.GetByIdAsync(cobrancaId)
            ?? throw new NotFoundException("Cobrança não encontrada.");

        if (cobranca.Status != StatusCobranca.PENDENTE)
            throw new DomainException("Só é possível alterar o método de pagamento de cobranças pendentes.");

        cobranca.AlterarMetodoPagamento(novoMetodo);

        await cobrancaRepo.UpdateAsync(cobranca);
        return CobrancaDto.FromEntity(cobranca);
    }

    public async Task<PaginatedResult<CobrancaDto>> ListarPorResponsavelAsync(
        Guid responsavelId, CobrancaFiltroDto? filtro = null, int page = 1, int pageSize = 10)
    {
        pageSize = Math.Clamp(pageSize, 1, 50);
        page = Math.Max(page, 1);

        var cobrancas = await cobrancaRepo.GetByResponsavelIdAsync(
            responsavelId, filtro?.Status, filtro?.VencidaApenas,
            filtro?.DataVencimentoDe, filtro?.DataVencimentoAte, page, pageSize);

        var total = await cobrancaRepo.CountByResponsavelIdAsync(
            responsavelId, filtro?.Status, filtro?.VencidaApenas,
            filtro?.DataVencimentoDe, filtro?.DataVencimentoAte);

        var items = cobrancas.Select(CobrancaDto.FromEntity).ToList();
        return PaginatedResult<CobrancaDto>.Create(items, page, pageSize, total);
    }

    public async Task<int> ContarPorResponsavelAsync(Guid responsavelId)
    {
        return await cobrancaRepo.CountByResponsavelIdAsync(responsavelId);
    }
}
