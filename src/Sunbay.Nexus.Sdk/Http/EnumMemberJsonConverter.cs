using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Http
{
    /// <summary>
    /// JSON converter for enums that supports EnumMember attribute
    /// </summary>
    internal class EnumMemberJsonConverter<T> : JsonConverter<T>
        where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (string.IsNullOrEmpty(stringValue))
                {
                    return default;
                }

                var enumType = typeof(T);

                // Try to find enum value by EnumMember attribute
                foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var enumMemberAttribute = field.GetCustomAttribute<EnumMemberAttribute>();
                    if (enumMemberAttribute != null && enumMemberAttribute.Value == stringValue)
                    {
                        return (T)field.GetValue(null)!;
                    }
                }

                // Fallback to default enum parsing
                if (Enum.TryParse<T>(stringValue, ignoreCase: true, out var result))
                {
                    return result;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var intValue = reader.GetInt32();
                var enumType = typeof(T);
                if (Enum.IsDefined(enumType, intValue))
                {
                    return (T)Enum.ToObject(enumType, intValue);
                }
            }

            throw new JsonException($"Unable to convert \"{reader.GetString()}\" to {typeof(T)}.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var enumType = typeof(T);
            var field = enumType.GetField(value.ToString()!);
            if (field != null)
            {
                var enumMemberAttribute = field.GetCustomAttribute<EnumMemberAttribute>();
                if (enumMemberAttribute != null)
                {
                    writer.WriteStringValue(enumMemberAttribute.Value);
                    return;
                }
            }

            // Fallback to default enum name
            writer.WriteStringValue(value.ToString());
        }
    }

    /// <summary>
    /// JSON converter for nullable enums that supports EnumMember attribute
    /// </summary>
    internal class NullableEnumMemberJsonConverter<T> : JsonConverter<T?>
        where T : struct, Enum
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (string.IsNullOrEmpty(stringValue))
                {
                    return null;
                }

                var enumType = typeof(T);

                // Try to find enum value by EnumMember attribute
                foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var enumMemberAttribute = field.GetCustomAttribute<EnumMemberAttribute>();
                    if (enumMemberAttribute != null && enumMemberAttribute.Value == stringValue)
                    {
                        return (T)field.GetValue(null)!;
                    }
                }

                // Fallback to default enum parsing
                if (Enum.TryParse<T>(stringValue, ignoreCase: true, out var result))
                {
                    return result;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var intValue = reader.GetInt32();
                var enumType = typeof(T);
                if (Enum.IsDefined(enumType, intValue))
                {
                    return (T)Enum.ToObject(enumType, intValue);
                }
            }

            throw new JsonException($"Unable to convert \"{reader.GetString()}\" to {typeof(T?)}.");
        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            var enumType = typeof(T);
            var field = enumType.GetField(value.Value.ToString()!);
            if (field != null)
            {
                var enumMemberAttribute = field.GetCustomAttribute<EnumMemberAttribute>();
                if (enumMemberAttribute != null)
                {
                    writer.WriteStringValue(enumMemberAttribute.Value);
                    return;
                }
            }

            // Fallback to default enum name
            writer.WriteStringValue(value.Value.ToString());
        }
    }

    /// <summary>
    /// Factory for creating EnumMemberJsonConverter instances
    /// </summary>
    internal class EnumMemberJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            Type enumType = typeToConvert.IsGenericType && 
                           typeToConvert.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? typeToConvert.GetGenericArguments()[0]
                : typeToConvert;

            return enumType.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert.IsGenericType && 
                typeToConvert.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type enumType = typeToConvert.GetGenericArguments()[0];
                Type converterType = typeof(NullableEnumMemberJsonConverter<>).MakeGenericType(enumType);
                return (JsonConverter)Activator.CreateInstance(converterType)!;
            }
            else
            {
                Type converterType = typeof(EnumMemberJsonConverter<>).MakeGenericType(typeToConvert);
                return (JsonConverter)Activator.CreateInstance(converterType)!;
            }
        }
    }
}

