namespace Fanex.Data
{
    using System.Collections.Generic;

    internal class ProxyParameter
    {
        public bool IgnoreRedundantParameters { get; set; }

        public DbSetting DbSetting { get; set; }

        public dynamic ParameterSet { get; set; }

        public List<string> TypeNames { get; set; } = new List<string>();
    }
}