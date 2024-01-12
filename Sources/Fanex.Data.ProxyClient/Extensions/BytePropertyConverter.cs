using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Fanex.Data.Extensions
{
    public class BytePropertyConverter : JsonConverter<byte[]>
    {
        public override bool CanWrite => false;

        public override byte[] ReadJson(JsonReader reader, Type objectType, byte[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Null)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                        {
                            var token = JToken.Load(reader);
                            return Convert.FromBase64String(token.ToString());
                        }
                    case JsonToken.StartArray:
                        {
                            var token = JToken.Load(reader);
                            var items = token.ToObject<List<byte>>();
                            return items.ToArray();
                        }
                }
            }

            return Array.Empty<byte>();
        }

        public override void WriteJson(JsonWriter writer, byte[] value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
