using Kedu.Application.DTOs;

namespace Kedu.API.GraphQL.Types;
public class PlanoType : ObjectType<PlanoDto>
{
    protected override void Configure(IObjectTypeDescriptor<PlanoDto> descriptor)
    {
        descriptor.Name("Plano");
        descriptor.Field(p => p.Id).Type<NonNullType<UuidType>>();
        descriptor.Field(p => p.ResponsavelId).Type<NonNullType<UuidType>>();
        descriptor.Field(p => p.CentroDeCustoId).Type<NonNullType<UuidType>>();
        descriptor.Field(p => p.ValorTotal).Type<NonNullType<DecimalType>>();
        descriptor.Field(p => p.Cobrancas).Type<NonNullType<ListType<NonNullType<CobrancaType>>>>();
    }
}
