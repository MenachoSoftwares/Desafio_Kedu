using FluentAssertions;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;

namespace Kedu.UnitTests;

public class PlanoTests
{
    [Fact]
    public void ValorTotal_DeveCalcularSomaDasCobrancas()
    {
        var plano = new PlanoDePagamento(Guid.NewGuid(), Guid.NewGuid());

        plano.AdicionarCobranca(100m, DateTime.UtcNow.AddDays(30), MetodoPagamento.BOLETO);
        plano.AdicionarCobranca(200m, DateTime.UtcNow.AddDays(60), MetodoPagamento.PIX);
        plano.AdicionarCobranca(150m, DateTime.UtcNow.AddDays(90), MetodoPagamento.BOLETO);

        plano.ValorTotal.Should().Be(450m);
    }

    [Fact]
    public void ValorTotal_DeveSerZero_QuandoSemCobrancas()
    {
        var plano = new PlanoDePagamento(Guid.NewGuid(), Guid.NewGuid());

        plano.ValorTotal.Should().Be(0m);
    }

    [Fact]
    public void Criar_DeveInicializarCorretamente()
    {
        var responsavelId = Guid.NewGuid();
        var centroId = Guid.NewGuid();

        var plano = new PlanoDePagamento(responsavelId, centroId);

        plano.Id.Should().NotBeEmpty();
        plano.ResponsavelId.Should().Be(responsavelId);
        plano.CentroDeCustoId.Should().Be(centroId);
        plano.Cobrancas.Should().BeEmpty();
    }

    [Fact]
    public void AdicionarCobranca_DeveAdicionarNaLista()
    {
        var plano = new PlanoDePagamento(Guid.NewGuid(), Guid.NewGuid());

        var cobranca = plano.AdicionarCobranca(500m, DateTime.UtcNow.AddDays(30), MetodoPagamento.PIX);

        plano.Cobrancas.Should().HaveCount(1);
        cobranca.PlanoId.Should().Be(plano.Id);
        cobranca.Valor.Should().Be(500m);
    }
}
