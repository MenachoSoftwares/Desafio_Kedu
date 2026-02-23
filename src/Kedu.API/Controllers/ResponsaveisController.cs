using Kedu.Application.DTOs;
using Kedu.Application.DTOs.Requests;
using Kedu.Application.Interfaces;
using Kedu.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Kedu.API.Controllers;

[ApiController]
[Route("responsaveis")]
public class ResponsaveisController : ControllerBase
{
    private readonly IResponsavelService responsavelService;

    public ResponsaveisController(IResponsavelService service) => responsavelService = service;
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarResponsavelRequest request)
    {
        var result = await responsavelService.CriarAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await responsavelService.ObterPorIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await responsavelService.ListarTodosAsync(page, pageSize);
        return Ok(result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarResponsavelRequest request)
    {
        var result = await responsavelService.AtualizarAsync(id, request);
        return Ok(result);
    }

    [HttpGet("{id}/planos-de-pagamento")]
    public async Task<IActionResult> ListarPlanos(Guid id, [FromServices] IPlanoService planoService)
    {
        var result = await planoService.ListarPorResponsavelAsync(id);
        return Ok(result);
    }

    [HttpGet("{id}/cobrancas")]
    public async Task<IActionResult> ListarCobrancas(
        Guid id,
        [FromQuery] StatusCobranca? status,
        [FromQuery] bool? vencidaApenas,
        [FromQuery] DateTime? dataVencimentoDe,
        [FromQuery] DateTime? dataVencimentoAte,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromServices] ICobrancaService? cobrancaService = null)
    {
        var filtro = new CobrancaFiltroDto
        {
            Status = status,
            VencidaApenas = vencidaApenas,
            DataVencimentoDe = dataVencimentoDe,
            DataVencimentoAte = dataVencimentoAte
        };

        var result = await cobrancaService!.ListarPorResponsavelAsync(id, filtro, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}/cobrancas/quantidade")]
    public async Task<IActionResult> QuantidadeCobrancas(Guid id, [FromServices] ICobrancaService cobrancaService)
    {
        var quantidade = await cobrancaService.ContarPorResponsavelAsync(id);
        return Ok(new { quantidade });
    }
}
