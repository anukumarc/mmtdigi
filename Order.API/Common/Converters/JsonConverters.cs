using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Order.API.Common.Converters
{
    // This custom converter is used to convert the output dates in 'dd-MMM-yyyy' format.
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToLocalTime().ToString("dd-MMM-yyyy"));
        }
    }
}
