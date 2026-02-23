using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kedu.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.CentrosDeCusto.AnyAsync())
            return; // Banco já tem informações

        // 1. Criar Centros de Custo
        var centroMatricula = new CentroDeCusto("Matrícula 2026", "Taxa de matrícula anual");
        var centroMensalidade = new CentroDeCusto("Mensalidade Escolar", "Mensalidades referentes ao ano letivo");
        var centroMaterial = new CentroDeCusto("Material Didático", "Apostilas e livros");

        context.CentrosDeCusto.AddRange(centroMatricula, centroMensalidade, centroMaterial);

        // 2. Criar Responsáveis Financeiros
        var responsavel1 = new ResponsavelFinanceiro(
            "João da Silva", "joao.silva@kedu.com", "11999998888", "52998224725");
        
        var responsavel2 = new ResponsavelFinanceiro(
            "Maria Oliveira", "maria.oliveira@kedu.com", "11977776666", "41315847065");

        context.Responsaveis.AddRange(responsavel1, responsavel2);

        // 3. Criar Planos de Pagamento com Cobranças
        var plano1 = new PlanoDePagamento(responsavel1.Id, centroMensalidade.Id);
        plano1.AdicionarCobranca(850.50m, DateTime.UtcNow.AddDays(-15), MetodoPagamento.PIX); // Atrasada (Vencida)
        plano1.AdicionarCobranca(850.50m, DateTime.UtcNow.AddDays(15), MetodoPagamento.BOLETO); // A vencer
        plano1.AdicionarCobranca(850.50m, DateTime.UtcNow.AddDays(45), MetodoPagamento.PIX);

        var plano2 = new PlanoDePagamento(responsavel2.Id, centroMatricula.Id);
        var cobrancaPaga = plano2.AdicionarCobranca(1200.00m, DateTime.UtcNow.AddDays(-5), MetodoPagamento.BOLETO);
        cobrancaPaga.RegistrarPagamento(); // Já Paga

        context.Planos.AddRange(plano1, plano2);

        await context.SaveChangesAsync();
    }
}
