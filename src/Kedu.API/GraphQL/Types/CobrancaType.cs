using Kedu.Application.DTOs;

namespace Kedu.API.GraphQL.Types;
public class CobrancaType : ObjectType<CobrancaDto>
{
    protected override void Configure(IObjectTypeDescriptor<CobrancaDto> descriptor)
    {
        descriptor.Name("Cobranca");
        descriptor.Field(c => c.Id).Type<NonNullType<UuidType>>();
        descriptor.Field(c => c.Valor).Type<NonNullType<DecimalType>>();
        descriptor.Field(c => c.EstaVencida).Type<NonNullType<BooleanType>>();
    }
}
