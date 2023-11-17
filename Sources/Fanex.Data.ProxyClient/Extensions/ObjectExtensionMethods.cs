namespace Fanex.Data.Extensions
{
    using System.Linq;

    internal static class ObjectExtensionMethods
    {
        internal static void CopyPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties().Where(x => x.CanRead).ToList();
            var toProperties = self.GetType().GetProperties()
                .Where(x => x.CanWrite)
                .ToList();

            foreach (var fromProperty in fromProperties)
            {
                if (toProperties.Exists(x => x.Name == fromProperty.Name))
                {
                    var p = toProperties.First(x => x.Name == fromProperty.Name);
                    if (p.CanWrite)
                    {
                        p.SetValue(self, fromProperty.GetValue(parent, null), null);
                    }
                }
            }
        }
    }
}