using FluentAssertions;
using Kedu.Application.DTOs.Requests;
using Kedu.Application.Services;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Domain.Interfaces;
using Moq;

namespace Kedu.UnitTests;

public class PlanoServiceTests
{
    private readonly Mock<IPlanoRepository> _planoRepoMock;
    private readonly Mock<IResponsavelRepository> _responsavelRepoMock;
    private readonly Mock<ICentroDeCustoRepository> _centroRepoMock;
    private readonly PlanoService _service;

    public PlanoServiceTests()
    {
        _planoRepoMock = new Mock<IPlanoRepository>();
        _responsavelRepoMock = new Mock<IResponsavelRepository>();
        _centroRepoMock = new Mock<ICentroDeCustoRepository>();
        _service = new PlanoService(
            _planoRepoMock.Object,
            _responsavelRepoMock.Object,
            _centroRepoMock.Object);
    }

    [Fact]
    public async Task CriarPlano_DeveCalcularValorTotalAutomaticamente()
    {
        var responsavelId = Guid.NewGuid();
        var centroId = Guid.NewGuid();

        _responsavelRepoMock
            .Setup(r => r.GetByIdAsync(responsavelId))
            .ReturnsAsync(new ResponsavelFinanceiro("João", "joao@test.com", "11999999999", "529.982.247-25"));

        _centroRepoMock
            .Setup(r => r.GetByIdAsync(centroId))
            .ReturnsAsync(new CentroDeCusto("Centro A", "Descrição"));

        var request = new CriarPlanoRequest
        {
            ResponsavelId = responsavelId,
            CentroDeCustoId = centroId,
            Cobrancas = new List<CriarCobrancaRequest>
            {
                new() { Valor = 100m, DataVencimento = DateTime.UtcNow.AddDays(30), MetodoPagamento = MetodoPagamento.BOLETO },
                new() { Valor = 200m, DataVencimento = DateTime.UtcNow.AddDays(60), MetodoPagamento = MetodoPagamento.PIX },
                new() { Valor = 150m, DataVencimento = DateTime.UtcNow.AddDays(90), MetodoPagamento = MetodoPagamento.BOLETO }
            }
        };
        var result = await _service.CriarPlanoAsync(request);
        result.ValorTotal.Should().Be(450m);
        result.Cobrancas.Should().HaveCount(3);
        _planoRepoMock.Verify(r => r.AddAsync(It.IsAny<PlanoDePagamento>()), Times.Once);
    }

    [Fact]
    public async Task CriarPlano_DeveLancarExcecao_QuandoResponsavelNaoExiste()
    {
        _responsavelRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((ResponsavelFinanceiro?)null);

        var request = new CriarPlanoRequest
        {
            ResponsavelId = Guid.NewGuid(),
            CentroDeCustoId = Guid.NewGuid(),
            Cobrancas = new List<CriarCobrancaRequest>
            {
                new() { Valor = 100m, DataVencimento = DateTime.UtcNow.AddDays(30), MetodoPagamento = MetodoPagamento.BOLETO }
            }
        };
        var act = () => _service.CriarPlanoAsync(request);
        await act.Should().ThrowAsync<Domain.Exceptions.NotFoundException>()
            .WithMessage("*Responsável*");
    }
}
