using Kedu.Application.DTOs;

namespace Kedu.API.GraphQL.Types;
public class ResponsavelType : ObjectType<ResponsavelDto>
{
    protected override void Configure(IObjectTypeDescriptor<ResponsavelDto> descriptor)
    {
        descriptor.Name("Responsavel");
        descriptor.Field(r => r.Id).Type<NonNullType<UuidType>>();
        descriptor.Field(r => r.Nome).Type<NonNullType<StringType>>();
        descriptor.Field(r => r.Email).Type<NonNullType<StringType>>();
    }
}
