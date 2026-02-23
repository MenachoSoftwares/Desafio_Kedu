using Kedu.Domain.Entities;

namespace Kedu.Application.DTOs;
public class CobrancaDto
{
    public Guid Id { get; set; }

    public Guid PlanoId { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataVencimento { get; set; }

    public string MetodoPagamento { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string CodigoPagamento { get; set; } = string.Empty;

    public bool EstaVencida { get; set; }

    public static CobrancaDto FromEntity(Cobranca entity)
    {
        return new CobrancaDto
        {
            Id = entity.Id,
            PlanoId = entity.PlanoId,
            Valor = entity.Valor,
            DataVencimento = entity.DataVencimento,
            MetodoPagamento = entity.MetodoPagamento.ToString(),
            Status = entity.Status.ToString(),
            CodigoPagamento = entity.CodigoPagamento,
            EstaVencida = entity.EstaVencida
        };
    }
}
