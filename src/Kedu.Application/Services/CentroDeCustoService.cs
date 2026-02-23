using Kedu.Application.DTOs;
using Kedu.Application.DTOs.Requests;
using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Domain.Exceptions;
using Kedu.Domain.Interfaces;

namespace Kedu.Application.Services;

public class CentroDeCustoService : ICentroDeCustoService
{
    private readonly ICentroDeCustoRepository repo;
    private readonly IPlanoRepository planoRepo;

    public CentroDeCustoService(ICentroDeCustoRepository repo, IPlanoRepository planoRepo)
    {
        this.repo = repo;
        this.planoRepo = planoRepo;
    }

    public async Task<CentroDeCustoDto> CriarAsync(CriarCentroDeCustoRequest request)
    {
        if (await repo.ExistsByNomeAsync(request.Nome))
            throw new DomainException("Já existe um centro de custo com este nome.");

        var centro = new CentroDeCusto(request.Nome, request.Descricao);
        await repo.AddAsync(centro);
        return CentroDeCustoDto.FromEntity(centro);
    }

    public async Task<CentroDeCustoDto?> ObterPorIdAsync(Guid id)
    {
        var centro = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException("Centro de custo não encontrado.");

        return CentroDeCustoDto.FromEntity(centro);
    }

    public async Task<PaginatedResult<CentroDeCustoDto>> ListarTodosAsync(int page = 1, int pageSize = 10)
    {
        pageSize = Math.Clamp(pageSize, 1, 50);
        page = Math.Max(page, 1);

        var lista = await repo.GetAllAsync(page, pageSize);
        var total = await repo.CountAsync();
        var items = lista.Select(CentroDeCustoDto.FromEntity).ToList();
        return PaginatedResult<CentroDeCustoDto>.Create(items, page, pageSize, total);
    }

    public async Task<CentroDeCustoDto> AtualizarAsync(Guid id, CriarCentroDeCustoRequest request)
    {
        var centro = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException("Centro de custo não encontrado.");

        if (await repo.ExistsByNomeAsync(request.Nome, id))
            throw new DomainException("Já existe um centro de custo com este nome.");

        centro.Atualizar(request.Nome, request.Descricao);
        await repo.UpdateAsync(centro);
        return CentroDeCustoDto.FromEntity(centro);
    }

    public async Task DeletarAsync(Guid id)
    {
        var centro = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException("Centro de custo não encontrado.");

        var possuiVinculos = await planoRepo.ExistsByCentroDeCustoIdAsync(id);
        if (possuiVinculos)
            throw new DomainException("Não é possível excluir um centro de custo que possui planos de pagamento vinculados.");

        await repo.DeleteAsync(centro);
    }
}
