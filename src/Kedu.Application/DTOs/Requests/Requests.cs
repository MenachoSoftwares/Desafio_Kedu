using Kedu.Domain.Enums;

namespace Kedu.Application.DTOs.Requests;

public class CriarResponsavelRequest
{
    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string CpfCnpj { get; set; } = string.Empty;
}

public class CriarCentroDeCustoRequest
{
    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;
}

public class CriarPlanoRequest
{
    public Guid ResponsavelId { get; set; }

    public Guid CentroDeCustoId { get; set; }

    public List<CriarCobrancaRequest> Cobrancas { get; set; } = new();
}

public class CriarCobrancaRequest
{
    public decimal Valor { get; set; }

    public DateTime DataVencimento { get; set; }

    public MetodoPagamento MetodoPagamento { get; set; }
}

public class AtualizarResponsavelRequest
{
    public string? Nome { get; set; }

    public string? Email { get; set; }

    public string? Telefone { get; set; }
}

public class AlterarVencimentoRequest
{
    public DateTime NovaDataVencimento { get; set; }
}

public class AlterarMetodoPagamentoRequest
{
    public MetodoPagamento NovoMetodoPagamento { get; set; }
}
