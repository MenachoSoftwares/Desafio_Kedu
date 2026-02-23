namespace Kedu.Domain.Entities;
public class PlanoDePagamento
{
    public Guid Id { get; private set; }

    public Guid ResponsavelId { get; private set; }

    public Guid CentroDeCustoId { get; private set; }

    private readonly List<Cobranca> cobrancas = new();

    public IReadOnlyCollection<Cobranca> Cobrancas => cobrancas.AsReadOnly();
    public decimal ValorTotal => cobrancas.Sum(c => c.Valor);
    protected PlanoDePagamento() { }

    public PlanoDePagamento(Guid responsavelId, Guid centroDeCustoId)
    {
        Id = Guid.NewGuid();
        ResponsavelId = responsavelId;
        CentroDeCustoId = centroDeCustoId;
    }
    public Cobranca AdicionarCobranca(decimal valor, DateTime dataVencimento, Enums.MetodoPagamento metodoPagamento)
    {
        var cobranca = new Cobranca(Id, valor, dataVencimento, metodoPagamento);
        cobrancas.Add(cobranca);
        return cobranca;
    }
}
