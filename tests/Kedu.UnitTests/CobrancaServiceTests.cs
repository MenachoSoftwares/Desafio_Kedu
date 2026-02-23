using FluentAssertions;
using Kedu.Application.DTOs.Requests;
using Kedu.Application.Services;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Domain.Exceptions;
using Kedu.Domain.Interfaces;
using Moq;

namespace Kedu.UnitTests;

public class CobrancaServiceTests
{
    private readonly Mock<ICobrancaRepository> _cobrancaRepoMock;
    private readonly CobrancaService _service;

    public CobrancaServiceTests()
    {
        _cobrancaRepoMock = new Mock<ICobrancaRepository>();
        _service = new CobrancaService(_cobrancaRepoMock.Object);
    }

    [Fact]
    public async Task CancelarCobrancaAsync_DeveRetornarDtoComStatusCancelada()
    {
        var cobranca = new Cobranca(
            Guid.NewGuid(), 200m, DateTime.UtcNow.AddDays(10), MetodoPagamento.BOLETO);

        _cobrancaRepoMock
            .Setup(r => r.GetByIdAsync(cobranca.Id))
            .ReturnsAsync(cobranca);
        var result = await _service.CancelarCobrancaAsync(cobranca.Id);
        result.Status.Should().Be("CANCELADA");
        _cobrancaRepoMock.Verify(r => r.UpdateAsync(cobranca), Times.Once);
    }

    [Fact]
    public async Task CancelarCobrancaAsync_DeveLancarExcecao_QuandoNaoEncontrada()
    {
        _cobrancaRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Cobranca?)null);

        var act = () => _service.CancelarCobrancaAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task RegistrarPagamentoAsync_DeveLancarExcecao_QuandoCobrancaCancelada()
    {
        var cobranca = new Cobranca(
            Guid.NewGuid(), 100m, DateTime.UtcNow.AddDays(10), MetodoPagamento.PIX);
        cobranca.Cancelar();

        _cobrancaRepoMock
            .Setup(r => r.GetByIdAsync(cobranca.Id))
            .ReturnsAsync(cobranca);
        var act = () => _service.RegistrarPagamentoAsync(cobranca.Id);
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*cancelada*");
    }
}
