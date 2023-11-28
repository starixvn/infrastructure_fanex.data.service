using Fanex.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Data
{
    public static class MapperExtension
    {
        public static IList<T> ToList<T>(this JToken token, Type type, JsonSerializer jsonSerializer)
        {
            return IsSimpleType(type) ? ToSimpleType<T>(token) : token.ToObject<IList<T>>(jsonSerializer);
        }

        public static T ToSingleData<T>(this JObject token, Type type, JsonSerializer jsonSerializer)
        {
            if (token is null)
            {
                return default;
            }

            return IsSimpleType(type) ? ToSimpleType<T>(token).First() : token.ToObject<T>(jsonSerializer);
        }

        private static IList<T> ToSimpleType<T>(this JToken token)
        {
            if (token is JObject)
            {
                var dictionary = token.ToObject<SimpleClass>();
                var value = dictionary.UnknownProperty.Values.First();
                return new List<T> { value.Value<T>() };
            }

            var dictionaries = token.ToObject<SimpleClass[]>().Select(v => v.UnknownProperty);
            var enumerable = dictionaries.Select(d => d.First().Value);
            return enumerable.Values<T>().ToList();
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type == typeof(decimal)
                   || type == typeof(string)
                   || type == typeof(DateTime)
                   || type == typeof(TimeSpan) || type == typeof(DateTimeOffset);
        }
    }
}
