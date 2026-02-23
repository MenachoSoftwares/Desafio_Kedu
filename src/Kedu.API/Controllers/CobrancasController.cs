using Kedu.Application.DTOs.Requests;
using Kedu.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kedu.API.Controllers;

[ApiController]
[Route("cobrancas")]
public class CobrancasController : ControllerBase
{
    private readonly ICobrancaService cobrancaService;

    public CobrancasController(ICobrancaService service) => cobrancaService = service;
    [HttpPost("{id}/pagar")]
    public async Task<IActionResult> RegistrarPagamento(Guid id)
    {
        var result = await cobrancaService.RegistrarPagamentoAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(Guid id)
    {
        var result = await cobrancaService.CancelarCobrancaAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id}/vencimento")]
    public async Task<IActionResult> AlterarVencimento(Guid id, [FromBody] AlterarVencimentoRequest request)
    {
        var result = await cobrancaService.AlterarVencimentoAsync(id, request.NovaDataVencimento);
        return Ok(result);
    }

    [HttpPatch("{id}/metodo-pagamento")]
    public async Task<IActionResult> AlterarMetodoPagamento(Guid id, [FromBody] AlterarMetodoPagamentoRequest request)
    {
        var result = await cobrancaService.AlterarMetodoPagamentoAsync(id, request.NovoMetodoPagamento);
        return Ok(result);
    }
}
