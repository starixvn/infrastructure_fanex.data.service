using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data.ProxyClient.Tests.Models;

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class Entity : IEquatable<Entity>
{
    private PropertyInfo[] properties;

    // ReSharper disable once MemberCanBePrivate.Global
    protected internal PropertyInfo[] Properties
    {
        get
        {
            properties ??= GetProperties(this);
            return properties;
        }
    }

    public override bool Equals(object obj)
        => Equals(obj as Entity);

    public virtual bool Equals(Entity other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other.Properties.Length != Properties.Length)
        {
            return false;
        }

        for (var propertyIndex = 0; propertyIndex < Properties.Length; propertyIndex++)
        {
            var value = GetValue(this, Properties[propertyIndex]);
            var otherValue = GetValue(other, other.Properties[propertyIndex]);

            if (!Equals(value, otherValue))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        return Properties.Select(property => GetValue(this, property) ?? string.Empty)
            .Aggregate(0, (current, value) => current ^ value.GetHashCode());
    }

    private static object GetValue(object target, PropertyInfo property)
        => property.GetValue(target, BindingFlags.Default, null, null, CultureInfo.InvariantCulture);

    private static PropertyInfo[] GetProperties(object target)
        => target
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => Attribute.GetCustomAttribute(property, typeof(IncomparablePropertyAttribute)) == null)
            .ToArray();
}