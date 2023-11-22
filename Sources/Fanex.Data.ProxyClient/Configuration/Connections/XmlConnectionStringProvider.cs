namespace Fanex.Data
{
    using System.Collections.Generic;

#if NETFRAMEWORK
    using System;
    using System.Configuration;
    using System.Linq;
#endif

    public class XmlConnectionStringProvider : ConnectionStringProviderBase
    {
        public XmlConnectionStringProvider(Dictionary<string, ConnectionConfiguration> connectionMaps)
        {
            ConnectionMaps = connectionMaps;
        }

#if NETFRAMEWORK

        public XmlConnectionStringProvider() => ConnectionMaps = ReadConnectionStrings();

        private static Dictionary<string, ConnectionConfiguration> ReadConnectionStrings()
        {
            if (ConfigurationManager.ConnectionStrings == null)
            {
                throw new ConfigurationErrorsException("Could not read the connection strings from your web.config or app.config.");
            }

            var connectionStringSettings = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>();

            return connectionStringSettings
                .Where(connectionStringSetting => !string.IsNullOrEmpty(connectionStringSetting.ConnectionString))
                .ToDictionary(
                    connectionStringSetting => connectionStringSetting.Name,
                    connectionStringSetting => new ConnectionConfiguration(
                        connectionStringSetting.Name,
                        connectionStringSetting.ConnectionString,
                        connectionStringSetting.ProviderName),
                    StringComparer.OrdinalIgnoreCase);
        }

#endif
    }
}