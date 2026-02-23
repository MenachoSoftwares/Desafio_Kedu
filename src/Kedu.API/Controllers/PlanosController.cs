using Kedu.Application.DTOs.Requests;
using Kedu.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kedu.API.Controllers;

[ApiController]
[Route("planos-de-pagamento")]
public class PlanosController : ControllerBase
{
    private readonly IPlanoService planoService;

    public PlanosController(IPlanoService service) => planoService = service;
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPlanoRequest request)
    {
        var result = await planoService.CriarPlanoAsync(request);
        return CreatedAtAction(nameof(Detalhe), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Detalhe(Guid id)
    {
        var result = await planoService.ObterPorIdAsync(id);
        return Ok(result);
    }

    [HttpGet("{id}/total")]
    public async Task<IActionResult> Total(Guid id)
    {
        var total = await planoService.ObterTotalAsync(id);
        return Ok(new { valorTotal = total });
    }
}
