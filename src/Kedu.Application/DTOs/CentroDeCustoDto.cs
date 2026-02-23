using Kedu.Domain.Entities;

namespace Kedu.Application.DTOs;
public class CentroDeCustoDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public static CentroDeCustoDto FromEntity(CentroDeCusto entity)
    {
        return new CentroDeCustoDto
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Descricao = entity.Descricao
        };
    }
}
