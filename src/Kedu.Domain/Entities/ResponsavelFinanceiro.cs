using Kedu.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Kedu.Domain.Entities;
public class ResponsavelFinanceiro
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string Telefone { get; private set; } = string.Empty;

    public string CpfCnpj { get; private set; } = string.Empty;
    protected ResponsavelFinanceiro() { }

    public ResponsavelFinanceiro(string nome, string email, string telefone, string cpfCnpj)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Telefone = telefone;
        CpfCnpj = SomenteDigitos(cpfCnpj);
    }
    public void AtualizarNome(string nome)
    {
        Nome = nome;
    }
    public void AtualizarEmail(string email)
    {
        Email = email;
    }
    public void AtualizarTelefone(string telefone)
    {
        Telefone = telefone;
    }



    private static string SomenteDigitos(string valor)
        => new(valor.Where(char.IsDigit).ToArray());
}
