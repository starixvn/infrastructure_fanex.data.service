namespace Fanex.Data.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ObjectExtensionMethods
{
    public static void CopyValueFrom(this object self, object source)
    {
        var fromProperties = source.GetType().GetProperties().Where(x => x.CanRead).ToList();
        var toProperties = self.GetType().GetProperties()
            .Where(x => x.CanWrite)
            .ToList();

        foreach (var toProperty in toProperties)
        {
            var fromProperty = fromProperties.First(x => x.Name == toProperty.Name);

            if (IsIgnored(fromProperty, source))
            {
                continue;
            }

            var value = fromProperty.GetValue(source, null);
            toProperty.SetValue(self, value, null);
        }
    }

    public static bool IsIgnored(PropertyInfo propertyInfo, object instance)
    {
        if (Attribute.GetCustomAttribute(propertyInfo, typeof(SpParamAttribute)) is not SpParamAttribute map)
        {
            return IsPropertyNull(propertyInfo, instance);
        }

        if (map.Ignored)
        {
            return true;
        }

        return !(map.AllowNull || !IsPropertyNull(propertyInfo, instance));
    }

    public static Dictionary<string, string> GetSpecialTypes(this object self)
    {
        var properties = self.GetType().GetProperties().Where(p =>
        {
            if (p.GetCustomAttributes(false).OfType<SpParamAttribute>().Any())
            {
                return p.GetCustomAttributes(false).OfType<SpParamAttribute>().FirstOrDefault(a => a.IsSpecialType) != null;
            }

            return false;
        });

        return properties.ToDictionary(p => p.Name, p => p.PropertyType.Name);
    }

    private static bool IsPropertyNull(PropertyInfo property, object instance)
    {
        if (instance == null || property == null)
        {
            return true;
        }

        var isStatic = property.GetGetMethod().IsStatic;

        var value = property.GetValue(isStatic ? null : instance);

        return value == null;
    }
}