using System.Text.Json;
using System.Text.Json.Serialization;

namespace DealerDatabase.Import.Parsing.Converters;

public class StringOrNumberConverter : JsonConverter<string?>
{
    public override string? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),

            JsonTokenType.Number => reader.GetInt64().ToString(),

            JsonTokenType.Null => null,

            _ => throw new JsonException(
                $"Expected a string or number, but found {reader.TokenType}.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        string? value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}