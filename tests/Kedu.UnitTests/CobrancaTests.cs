using FluentAssertions;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Domain.Exceptions;

namespace Kedu.UnitTests;

public class CobrancaTests
{
    [Fact]
    public void Criar_DeveGerarCodigoBoleto()
    {
        var cobranca = new Cobranca(
            Guid.NewGuid(), 100m, DateTime.UtcNow.AddDays(30), MetodoPagamento.BOLETO);
        cobranca.CodigoPagamento.Should().Contain(".");
        cobranca.CodigoPagamento.Should().Contain(" ");
        cobranca.Status.Should().Be(StatusCobranca.PENDENTE);
    }

    [Fact]
    public void Criar_DeveGerarCodigoPix()
    {
        var cobranca = new Cobranca(
            Guid.NewGuid(), 50m, DateTime.UtcNow.AddDays(15), MetodoPagamento.PIX);
        cobranca.CodigoPagamento.Should().StartWith("00020126580014BR.GOV.BCB.PIX");
        cobranca.Status.Should().Be(StatusCobranca.PENDENTE);
    }

    [Fact]
    public void RegistrarPagamento_DeveAlterarStatusParaPaga()
    {
        var cobranca = new Cobranca(
            Guid.NewGuid(), 200m, DateTime.UtcNow.AddDays(10), MetodoPagamento.BOLETO);

        cobranca.RegistrarPagamento();

        cobranca.Status.Should().Be(StatusCobranca.PAGA);
    }



    [Fact]
    public void EstaVencida_DeveRetornarTrue_QuandoPassouDoVencimento()
    {
        var cobranca = new Cobranca(
            Guid.NewGuid(), 100m, DateTime.UtcNow.AddDays(-1), MetodoPagamento.PIX);

        cobranca.EstaVencida.Should().BeTrue();
    }

    [Fact]
    public void EstaVencida_DeveRetornarFalse_QuandoPaga()
    {
        var cobranca = new Cobranca(
            Guid.NewGuid(), 100m, DateTime.UtcNow.AddDays(-1), MetodoPagamento.PIX);

        cobranca.RegistrarPagamento();

        cobranca.EstaVencida.Should().BeFalse();
    }

    [Fact]
    public void EstaVencida_DeveRetornarFalse_QuandoAindaNaoVenceu()
    {
        var cobranca = new Cobranca(
            Guid.NewGuid(), 100m, DateTime.UtcNow.AddDays(30), MetodoPagamento.PIX);

        cobranca.EstaVencida.Should().BeFalse();
    }

    [Fact]
    public void EstaVencida_DeveRetornarFalse_QuandoCancelada()
    {
        var cobranca = new Cobranca(
            Guid.NewGuid(), 100m, DateTime.UtcNow.AddDays(-1), MetodoPagamento.PIX);

        cobranca.Cancelar();

        cobranca.EstaVencida.Should().BeFalse();
    }
}
