using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Maestrano.Api
{
    /// <summary>
    /// Corrected version of IsoDateTimeConverter that accept date written in ISO_8601 format with a negative or zero year
    /// Since DateTime does not support such date, returns null or DateTime.MinValue depending if it is a DateTime? or DateTime that is deserialized
    /// https://en.wikipedia.org/wiki/ISO_8601
    /// </summary>
    class CorrectedIsoDateTimeConverter : IsoDateTimeConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string dateText = reader.Value.ToString();
                if (dateText.StartsWith("0000") || dateText.StartsWith("-"))
                {
                    if (IsNullable(objectType))
                    {
                        return null;
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }

                }
            }
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
        private static bool IsNullable(Type t)
        {
            if (t.IsValueType)
            {
                return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
            }
            else
            {
                return true;
            }
        }
    }
}
