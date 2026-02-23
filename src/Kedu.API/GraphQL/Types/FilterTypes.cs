using Kedu.Application.DTOs;
using Kedu.Domain.Enums;

namespace Kedu.API.GraphQL.Types;
public class CobrancaFiltroInputType : InputObjectType<CobrancaFiltroDto>
{
    protected override void Configure(IInputObjectTypeDescriptor<CobrancaFiltroDto> descriptor)
    {
        descriptor.Name("CobrancaFiltroInput");

        descriptor.Field(f => f.Status)
            .Type<EnumType<StatusCobranca>>()
            .Description("Filtra pelo status da cobrança (PENDENTE, PAGA, CANCELADA).");

        descriptor.Field(f => f.VencidaApenas)
            .Type<BooleanType>()
            .Description("Se true, retorna apenas as cobranças vencidas.");

        descriptor.Field(f => f.DataVencimentoDe)
            .Type<DateTimeType>()
            .Description("Filtro de data de vencimento (início).");

        descriptor.Field(f => f.DataVencimentoAte)
            .Type<DateTimeType>()
            .Description("Filtro de data de vencimento (fim).");
    }
}
