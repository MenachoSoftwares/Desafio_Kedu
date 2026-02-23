using Kedu.Domain.Entities;

namespace Kedu.Application.DTOs;
public class ResponsavelDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string CpfCnpj { get; set; } = string.Empty;

    public static ResponsavelDto FromEntity(ResponsavelFinanceiro entity)
    {
        return new ResponsavelDto
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Email = entity.Email,
            Telefone = entity.Telefone,
            CpfCnpj = entity.CpfCnpj
        };
    }
}
