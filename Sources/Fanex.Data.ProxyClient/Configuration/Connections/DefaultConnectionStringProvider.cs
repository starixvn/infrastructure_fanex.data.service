namespace Fanex.Data
{
    using System.Collections.Generic;
    
    public class DefaultConnectionStringProvider : ConnectionStringProviderBase
    {
        public DefaultConnectionStringProvider(Dictionary<string, ConnectionConfiguration> connectionMaps)
        {
            ConnectionMaps = connectionMaps;
        }
    }
}