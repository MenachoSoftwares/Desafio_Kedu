namespace Kedu.Domain.Entities;
public class Pagamento
{
    public Guid Id { get; private set; }
    public Guid CobrancaId { get; private set; }
    public DateTime DataPagamento { get; private set; }
    public decimal ValorPago { get; private set; }

    private Pagamento() { }

    public static Pagamento Criar(Guid cobrancaId, decimal valorPago)
    {
        return new Pagamento
        {
            Id = Guid.NewGuid(),
            CobrancaId = cobrancaId,
            DataPagamento = DateTime.UtcNow,
            ValorPago = valorPago
        };
    }
}
