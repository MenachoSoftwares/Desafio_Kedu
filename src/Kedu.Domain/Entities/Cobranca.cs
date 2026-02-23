using Kedu.Domain.Enums;
using Kedu.Domain.Exceptions;

namespace Kedu.Domain.Entities;
public class Cobranca
{
    public Guid Id { get; private set; }

    public Guid PlanoId { get; private set; }

    public decimal Valor { get; private set; }

    public DateTime DataVencimento { get; private set; }

    public MetodoPagamento MetodoPagamento { get; private set; }

    public StatusCobranca Status { get; private set; }

    public string CodigoPagamento { get; private set; } = string.Empty;
    public bool EstaVencida =>
        Status != StatusCobranca.PAGA &&
        Status != StatusCobranca.CANCELADA &&
        DateTime.UtcNow > DataVencimento;
    protected Cobranca() { }

    public Cobranca(Guid planoId, decimal valor, DateTime dataVencimento, MetodoPagamento metodoPagamento)
    {
        Id = Guid.NewGuid();
        PlanoId = planoId;
        Valor = valor;
        DataVencimento = dataVencimento;
        MetodoPagamento = metodoPagamento;
        Status = StatusCobranca.PENDENTE;

        GerarCodigoPagamento();
    }
    private void GerarCodigoPagamento()
    {
        var hash = Guid.NewGuid().ToString("N");
        CodigoPagamento = MetodoPagamento switch
        {
            MetodoPagamento.BOLETO => GerarLinhaDigitavelSimulada(hash),
            MetodoPagamento.PIX => GerarPixCopiaCola(hash),
            _ => throw new InvalidOperationException("Método de pagamento inválido.")
        };
    }

    private static string GerarLinhaDigitavelSimulada(string hash)
    {
        var digitos = string.Concat(hash.Where(char.IsDigit)).PadRight(47, '0')[..47];
        return $"{digitos[..5]}.{digitos[5..10]} " +
               $"{digitos[10..15]}.{digitos[15..21]} " +
               $"{digitos[21..26]}.{digitos[26..32]} " +
               $"{digitos[32]} " +
               $"{digitos[33..47]}";
    }

    private static string GerarPixCopiaCola(string hash)
    {
        var chave = hash[..32];
        return $"00020126580014BR.GOV.BCB.PIX0136{chave}5204000053039865802BR5913KEDU*MOCK6014CIDADE*MOCK62070503***6304ABCD";
    }
    public void RegistrarPagamento()
    {
        Status = StatusCobranca.PAGA;
    }
    public void Cancelar()
    {
        Status = StatusCobranca.CANCELADA;
    }
    public void AlterarVencimento(DateTime novaDataVencimento)
    {
        DataVencimento = novaDataVencimento;
    }
    public void AlterarMetodoPagamento(MetodoPagamento novoMetodo)
    {
        MetodoPagamento = novoMetodo;
        GerarCodigoPagamento(); // Regenera código com o novo método
    }
}
