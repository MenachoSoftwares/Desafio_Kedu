using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kedu.API.Converters;
public class FlexibleDateTimeConverter : JsonConverter<DateTime>
{
    private static readonly string[] _formats =
    [
        "dd/MM/yyyy",
        "dd-MM-yyyy",
        "yyyy-MM-dd",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-ddTHH:mm:ss.fffZ",
        "yyyy-MM-ddTHH:mm:ss.fffffffZ",
    ];

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            throw new JsonException("O valor da data não pode ser vazio.");

        if (DateTime.TryParseExact(value, _formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);

        throw new JsonException(
            $"Formato de data inválido: '{value}'. Use um dos formatos aceitos: dd/MM/yyyy, dd-MM-yyyy ou yyyy-MM-dd.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}
