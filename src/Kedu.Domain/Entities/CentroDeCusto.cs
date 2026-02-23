namespace Kedu.Domain.Entities;
public class CentroDeCusto
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; } = string.Empty;

    public string Descricao { get; private set; } = string.Empty;
    protected CentroDeCusto() { }

    public CentroDeCusto(string nome, string descricao)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
    }
    public void Atualizar(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}
