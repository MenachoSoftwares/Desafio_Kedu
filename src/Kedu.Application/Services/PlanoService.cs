using Kedu.Application.DTOs;
using Kedu.Application.DTOs.Requests;
using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Domain.Exceptions;
using Kedu.Domain.Interfaces;

namespace Kedu.Application.Services;

public class PlanoService : IPlanoService
{
    private readonly IPlanoRepository planoRepo;
    private readonly IResponsavelRepository responsavelRepo;
    private readonly ICentroDeCustoRepository centroRepo;

    public PlanoService(
        IPlanoRepository planoRepo,
        IResponsavelRepository responsavelRepo,
        ICentroDeCustoRepository centroRepo)
    {
        this.planoRepo = planoRepo;
        this.responsavelRepo = responsavelRepo;
        this.centroRepo = centroRepo;
    }

    public async Task<PlanoDto> CriarPlanoAsync(CriarPlanoRequest request)
    {
        var responsavel = await responsavelRepo.GetByIdAsync(request.ResponsavelId)
            ?? throw new NotFoundException("Responsável não encontrado.");

        var centro = await centroRepo.GetByIdAsync(request.CentroDeCustoId)
            ?? throw new NotFoundException("Centro de custo não encontrado.");
        var plano = new PlanoDePagamento(responsavel.Id, centro.Id);
        foreach (var cobranca in request.Cobrancas)
        {
            plano.AdicionarCobranca(cobranca.Valor, cobranca.DataVencimento, cobranca.MetodoPagamento);
        }

        await planoRepo.AddAsync(plano);
        return PlanoDto.FromEntity(plano);
    }

    public async Task<PlanoDto?> ObterPorIdAsync(Guid id)
    {
        var plano = await planoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Plano de pagamento não encontrado.");

        return PlanoDto.FromEntity(plano);
    }

    public async Task<decimal> ObterTotalAsync(Guid id)
    {
        var plano = await planoRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Plano de pagamento não encontrado.");

        return plano.ValorTotal;
    }

    public async Task<List<PlanoDto>> ListarPorResponsavelAsync(Guid responsavelId)
    {
        var planos = await planoRepo.GetByResponsavelIdAsync(responsavelId);
        return planos.Select(PlanoDto.FromEntity).ToList();
    }
}
