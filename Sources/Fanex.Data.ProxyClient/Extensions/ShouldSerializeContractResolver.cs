using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Fanex.Data.Extensions;

using System;

public class ShouldSerializeContractResolver : DefaultContractResolver
{
    public static readonly ShouldSerializeContractResolver Instance = new();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (member is not PropertyInfo)
        {
            return property;
        }

        property.ShouldSerialize = PredictShouldSerialize(member);
        property.PropertyName = GetPropertyName(member, property);
        property.NullValueHandling = GetNullValueHandling(member);
        return property;
    }

    private static Predicate<object> PredictShouldSerialize(MemberInfo member)
    {
        if (Attribute.GetCustomAttribute(member, typeof(SpParamAttribute)) is not SpParamAttribute map)
        {
            return instance => !IsPropertyNull(instance, (PropertyInfo)member);
        }

        return instance =>
        {
            if (map.Ignored)
            {
                return false;
            }

            return map.AllowNull || !IsPropertyNull(instance, (PropertyInfo)member);
        };
    }

    private static string GetPropertyName(MemberInfo member, JsonProperty property)
    {
        if (Attribute.GetCustomAttribute(member, typeof(SpParamAttribute)) is not SpParamAttribute map || string.IsNullOrWhiteSpace(map.SqlParamName))
        {
            return property.PropertyName;
        }

        return map.SqlParamName;
    }

    private static bool IsPropertyNull(object instance, PropertyInfo property)
    {
        if (instance == null || property == null)
        {
            return true;
        }

        var isStatic = property.GetGetMethod().IsStatic;

        var value = property.GetValue(isStatic ? null : instance);

        return value == null;
    }

    private static NullValueHandling GetNullValueHandling(MemberInfo member)
    {
        return Attribute.GetCustomAttribute(member, typeof(SpParamAttribute)) is not SpParamAttribute { AllowNull: true } ? NullValueHandling.Ignore : NullValueHandling.Include;
    }
}