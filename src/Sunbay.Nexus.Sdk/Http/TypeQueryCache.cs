using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Http
{
    /// <summary>
    /// Cache of pre-compiled property getters for request DTOs used to build
    /// URL query strings. Reflection metadata (property list, JSON name, getter
    /// delegate) is captured once per type; hot path executes only strongly-typed delegates.
    /// </summary>
    internal static class TypeQueryCache
    {
        private static readonly ConcurrentDictionary<Type, (Func<object, object?> Getter, string JsonName)[]> Cache = new();

        public static (Func<object, object?> Getter, string JsonName)[] GetProperties(Type type)
        {
            return Cache.GetOrAdd(type, static t => Build(t));
        }

        private static (Func<object, object?> Getter, string JsonName)[] Build(Type type)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var list = new List<(Func<object, object?>, string)>(props.Length);

            foreach (var property in props)
            {
                if (!property.CanRead || property.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                // Respect [JsonIgnore]
                if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                {
                    continue;
                }

                // Prefer [JsonPropertyName] to keep the same wire-format as JSON body
                var jsonAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                var jsonName = jsonAttr?.Name ?? JsonNamingPolicy.CamelCase.ConvertName(property.Name);

                list.Add((BuildGetter(property), jsonName));
            }

            return list.ToArray();
        }

        private static Func<object, object?> BuildGetter(PropertyInfo property)
        {
            // (object instance) => (object)((TDeclaring)instance).Property
            var instance = Expression.Parameter(typeof(object), "instance");
            var typed = Expression.Convert(instance, property.DeclaringType!);
            var access = Expression.Property(typed, property);
            var boxed = Expression.Convert(access, typeof(object));
            return Expression.Lambda<Func<object, object?>>(boxed, instance).Compile();
        }
    }
}
