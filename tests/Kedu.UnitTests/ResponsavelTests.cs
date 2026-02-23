using FluentAssertions;
using Kedu.Domain.Entities;
using Kedu.Domain.Exceptions;

namespace Kedu.UnitTests;

public class ResponsavelTests
{
    private const string CpfValido = "529.982.247-25";
    private const string CpfValidoSemMascara = "52998224725";
    private const string CnpjValido = "11.222.333/0001-81";

    [Fact]
    public void Criar_DeveAceitar_CpfValido()
    {
        var responsavel = new ResponsavelFinanceiro("João", "joao@email.com", "11999999999", CpfValido);

        responsavel.Nome.Should().Be("João");
        responsavel.CpfCnpj.Should().Be(CpfValidoSemMascara);
    }

    [Fact]
    public void Criar_DeveAceitar_CpfSemMascara()
    {
        var responsavel = new ResponsavelFinanceiro("Maria", "maria@email.com", "11988888888", CpfValidoSemMascara);

        responsavel.CpfCnpj.Should().Be(CpfValidoSemMascara);
    }

    [Fact]
    public void Criar_DeveAceitar_CnpjValido()
    {
        var responsavel = new ResponsavelFinanceiro("Empresa", "empresa@email.com", "1140001234", CnpjValido);

        responsavel.CpfCnpj.Should().Be("11222333000181");
    }


}
