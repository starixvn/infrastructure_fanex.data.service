namespace Fanex.Data.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ObjectExtensionMethods
    {
        public static void CopyValueFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties().Where(x => x.CanRead).ToList();
            var toProperties = self.GetType().GetProperties()
                .Where(x => x.CanWrite)
                .ToList();

            foreach (var toProperty in toProperties)
            {
                var fromProperty = fromProperties.First(x => x.Name == toProperty.Name);

                if (IsIgnored(fromProperty, parent))
                {
                    continue;
                }

                var value = fromProperty.GetValue(parent, null);
                toProperty.SetValue(self,value, null);
            }
        }

        public static bool IsIgnored(PropertyInfo propertyInfo, object instance)
        {
            if (!(Attribute.GetCustomAttribute(propertyInfo, typeof(SpParamAttribute)) is SpParamAttribute map))
            {
                return IsPropertyNull(instance, propertyInfo);
            }

            if (map.Ignored)
            {
                return true;
            }

            return !(map.AllowNull || !IsPropertyNull(instance, propertyInfo));
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
    }
}