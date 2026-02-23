using Kedu.Application.DTOs.Requests;
using Kedu.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kedu.API.Controllers;

[ApiController]
[Route("centros-de-custo")]
public class CentrosDeCustoController : ControllerBase
{
    private readonly ICentroDeCustoService centroDeCustoService;

    public CentrosDeCustoController(ICentroDeCustoService service) => centroDeCustoService = service;

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarCentroDeCustoRequest request)
    {
        var result = await centroDeCustoService.CriarAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await centroDeCustoService.ObterPorIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await centroDeCustoService.ListarTodosAsync(page, pageSize);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] CriarCentroDeCustoRequest request)
    {
        var result = await centroDeCustoService.AtualizarAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await centroDeCustoService.DeletarAsync(id);
        return NoContent();
    }
}
