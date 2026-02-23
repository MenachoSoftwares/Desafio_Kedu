using Kedu.Application.DTOs;
using Kedu.Application.DTOs.Requests;
using Kedu.Application.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Domain.Exceptions;
using Kedu.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace Kedu.Application.Services;

public class ResponsavelService : IResponsavelService
{
    private readonly IResponsavelRepository responsavelRepository;

    public ResponsavelService(IResponsavelRepository repo) => responsavelRepository = repo;
    public async Task<ResponsavelDto> CriarAsync(CriarResponsavelRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new DomainException("O nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Telefone))
            throw new DomainException("O telefone não pode ser vazio.");

        ValidarEmail(request.Email);
        ValidarCpfCnpj(request.CpfCnpj);

        var cpfCnpjLimpo = SomenteDigitos(request.CpfCnpj);
        if (await responsavelRepository.ExistsByCpfCnpjAsync(cpfCnpjLimpo))
            throw new DomainException($"Já existe um responsável cadastrado com o CPF/CNPJ informado.");

        var responsavel = new ResponsavelFinanceiro(
            request.Nome, request.Email, request.Telefone, request.CpfCnpj);

        await responsavelRepository.AddAsync(responsavel);
        return ResponsavelDto.FromEntity(responsavel);
    }

    public async Task<ResponsavelDto?> ObterPorIdAsync(Guid id)
    {
        var responsavel = await responsavelRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Responsável não encontrado.");

        return ResponsavelDto.FromEntity(responsavel);
    }

    public async Task<PaginatedResult<ResponsavelDto>> ListarTodosAsync(int page = 1, int pageSize = 10)
    {
        pageSize = Math.Clamp(pageSize, 1, 50);
        page = Math.Max(page, 1);

        var lista = await responsavelRepository.GetAllAsync(page, pageSize);
        var total = await responsavelRepository.CountAsync();
        var items = lista.Select(ResponsavelDto.FromEntity).ToList();
        return PaginatedResult<ResponsavelDto>.Create(items, page, pageSize, total);
    }

    public async Task<ResponsavelDto> AtualizarAsync(Guid id, AtualizarResponsavelRequest request)
    {
        var responsavel = await responsavelRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Responsável não encontrado.");

        if (request.Nome != null)
        {
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new DomainException("O nome não pode ser vazio.");
            responsavel.AtualizarNome(request.Nome);
        }

        if (request.Email != null)
        {
            ValidarEmail(request.Email);
            responsavel.AtualizarEmail(request.Email);
        }

        if (request.Telefone != null)
        {
            if (string.IsNullOrWhiteSpace(request.Telefone))
                throw new DomainException("O telefone não pode ser vazio.");
            responsavel.AtualizarTelefone(request.Telefone);
        }

        await responsavelRepository.UpdateAsync(responsavel);
        return ResponsavelDto.FromEntity(responsavel);
    }

    private static void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("O e-mail é obrigatório.");

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new DomainException("O e-mail informado não é válido.");
    }

    private static void ValidarCpfCnpj(string cpfCnpj)
    {
        if (string.IsNullOrWhiteSpace(cpfCnpj))
            throw new DomainException("O CPF/CNPJ é obrigatório.");

        var digitos = SomenteDigitos(cpfCnpj);

        if (digitos.Length == 11)
            ValidarCpf(digitos);
        else if (digitos.Length == 14)
            ValidarCnpj(digitos);
        else
            throw new DomainException("CPF deve ter 11 dígitos e CNPJ deve ter 14 dígitos.");
    }

    private static void ValidarCpf(string cpf)
    {
        if (cpf.Distinct().Count() == 1)
            throw new DomainException("CPF inválido.");

        var soma = 0;
        for (var i = 0; i < 9; i++)
            soma += (cpf[i] - '0') * (10 - i);

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        if (cpf[9] - '0' != digito1)
            throw new DomainException("CPF inválido.");

        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += (cpf[i] - '0') * (11 - i);

        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        if (cpf[10] - '0' != digito2)
            throw new DomainException("CPF inválido.");
    }

    private static void ValidarCnpj(string cnpj)
    {
        if (cnpj.Distinct().Count() == 1)
            throw new DomainException("CNPJ inválido.");

        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var soma = 0;
        for (var i = 0; i < 12; i++)
            soma += (cnpj[i] - '0') * multiplicador1[i];

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        if (cnpj[12] - '0' != digito1)
            throw new DomainException("CNPJ inválido.");

        soma = 0;
        for (var i = 0; i < 13; i++)
            soma += (cnpj[i] - '0') * multiplicador2[i];

        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        if (cnpj[13] - '0' != digito2)
            throw new DomainException("CNPJ inválido.");
    }

    private static string SomenteDigitos(string valor)
        => new(valor.Where(char.IsDigit).ToArray());
}
