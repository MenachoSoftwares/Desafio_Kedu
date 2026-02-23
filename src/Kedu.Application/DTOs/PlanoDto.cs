using Kedu.Domain.Entities;

namespace Kedu.Application.DTOs;
public class PlanoDto
{
    public Guid Id { get; set; }

    public Guid ResponsavelId { get; set; }

    public Guid CentroDeCustoId { get; set; }

    public decimal ValorTotal { get; set; }

    public List<CobrancaDto> Cobrancas { get; set; } = new();

    public static PlanoDto FromEntity(PlanoDePagamento entity)
    {
        return new PlanoDto
        {
            Id = entity.Id,
            ResponsavelId = entity.ResponsavelId,
            CentroDeCustoId = entity.CentroDeCustoId,
            ValorTotal = entity.ValorTotal,
            Cobrancas = entity.Cobrancas.Select(CobrancaDto.FromEntity).ToList(),
        };
    }
}
