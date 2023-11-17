using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class XmlDbSettingProvider : DbSettingProviderBase
    {
        private readonly ConcurrentDictionary<string, DbSetting> settingDataSource
            = new ConcurrentDictionary<string, DbSetting>(StringComparer.OrdinalIgnoreCase);

        public XmlDbSettingProvider(string resourcePath)
            : base(resourcePath)
        {
        }

        public XmlDbSettingProvider(string resourcePath, Dictionary<string, ConnectionConfiguration> connectionMaps)
            : base(resourcePath)
        {
            ConnectionStringProvider = new XmlConnectionStringProvider(connectionMaps);
        }

        public override IDictionary<string, DbSetting> SettingDataSource => settingDataSource;

        public override DbSettingProviderBase DeleteSetting(DbSetting setting)
        {
            settingDataSource.TryRemove(setting.Key, out _);

            return this;
        }

        public override DbSettingProviderBase InsertSetting(DbSetting setting)
        {
            settingDataSource.TryAdd(setting.Key, setting);

            return this;
        }

        public override DbSettingProviderBase UpdateSetting(DbSetting setting)
        {
            if (settingDataSource.TryGetValue(setting.Key, out var oldSetting))
            {
                settingDataSource.TryUpdate(setting.Key, setting, oldSetting);
            }

            return this;
        }

        public override void Start()
            => LoadSettingDataSource(ResourcePath);

        protected static bool IsEncryptedConnection(XElement element)
        {
            var attr = GetXmlAttributeValue(element.Attribute("Plain"));

            return !string.Equals(attr, "1", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(attr, "true", StringComparison.OrdinalIgnoreCase);
        }

        protected static bool IsXmlFile(string path)
            => string.Equals(Path.GetExtension(path), ".xml", StringComparison.OrdinalIgnoreCase);

        protected static string[] GetResourceFilePaths(string resourcePath)
        {
            string[] resourceFiles = null;

            if (IsFolder(resourcePath))
            {
                resourceFiles = Directory.GetFiles(resourcePath).Where(IsXmlFile).ToArray();
            }
            else if (IsXmlFile(resourcePath))
            {
                resourceFiles = new[] { resourcePath };
            }

            if (resourceFiles == null || resourceFiles.Length == 0)
            {
                throw new FileNotFoundException($"No XML file was found at the given path: '{resourcePath}'.");
            }

            return resourceFiles;
        }

        protected static void ThrowInvalidXmlFormatException(XElement setting)
        {
            const string exceptionMessage = "The Stored Procedure configuration is incorrect.";

            IXmlLineInfo line = (setting as IXmlLineInfo).HasLineInfo() ? setting : null;
            var innerException = new XmlSchemaException(
                "Key, DbName, and SpName are all required. If any Mapping element defines Class attribute, then it is required for the rest.");

            if (line != null)
            {
                throw new XmlSchemaValidationException(exceptionMessage, innerException, line.LineNumber, line.LinePosition);
            }

            throw new XmlSchemaValidationException(exceptionMessage, innerException);
        }

        protected static string GetXmlAttributeValue(XAttribute attribute)
            => attribute?.Value.Trim();

        protected virtual void LoadSettingDataSource(string resourcePath, bool overwrite = false)
        {
            var resourceFiles = GetResourceFilePaths(resourcePath);

            foreach (var filePath in resourceFiles)
            {
                using (var streamReader = CreateStreamReader(filePath))
                {
                    var xmlContent = streamReader.ReadToEnd();
                    var document = XDocument.Parse(xmlContent, LoadOptions.SetLineInfo);
                    var settings = document.Descendants("Setting");

                    foreach (var setting in settings)
                    {
                        var dbSetting = ParseDbSetting(setting);

                        if (!dbSetting.IsValid())
                        {
                            ThrowInvalidXmlFormatException(setting);
                        }

                        if (!settingDataSource.ContainsKey(dbSetting.Key) || overwrite)
                        {
                            settingDataSource.AddOrUpdate(
                                dbSetting.Key,
                                dbSetting,
                                (k, v) => dbSetting);
                        }
                    }
                }
            }
        }

        private static StreamReader CreateStreamReader(string filePath)
        {
            return new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        protected virtual DbSetting ParseDbSetting(XElement setting)
        {
            var isEncryptedConnection = IsEncryptedConnection(setting);
            var key = GetXmlAttributeValue(setting.Attribute("Key"));
            var dbName = GetXmlAttributeValue(setting.Attribute("DbName"));
            var providerName = ConnectionStringProvider.GetProviderName(dbName);
            var connectionString = ConnectionStringProvider.GetConnectionString(dbName, isEncryptedConnection);
            var spName = GetXmlAttributeValue(setting.Attribute("SpName"));
            var timeout = GetXmlAttributeValue(setting.Attribute("Timeout") ?? setting.Attribute("TimeOut"));
            var columnMaps = ParseColumnMaps(setting.Descendants("Mapping"));
            var parameters = ParseFirmParameters(setting.Descendants("Parameter"));

            return int.TryParse(timeout, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var timeoutInSeconds)
                ? new DbSetting(key, providerName, dbName, connectionString, spName, timeoutInSeconds, columnMaps, parameters)
                : new DbSetting(key, providerName, dbName, connectionString, spName, null, columnMaps, parameters);
        }

        protected virtual IDictionary<string, string> ParseColumnMaps(IEnumerable<XElement> columnMaps)
        {
            var customMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var columnMap in columnMaps)
            {
                var dataField = GetXmlAttributeValue(columnMap.Attribute("DataField"));
                var objectProperty = GetXmlAttributeValue(columnMap.Attribute("ObjectProperty"));
                var typeName = GetXmlAttributeValue(columnMap.Attribute("Class"));
                var mappingKey = string.IsNullOrEmpty(typeName) ? dataField : $"{dataField};{typeName}";

                if (!string.IsNullOrEmpty(mappingKey) && !string.IsNullOrEmpty(objectProperty))
                {
                    customMapping[mappingKey] = objectProperty;
                }
            }

            return customMapping;
        }

        protected virtual IDictionary<string, object> ParseFirmParameters(IEnumerable<XElement> parameters)
        {
            var firmParameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (var parameter in parameters)
            {
                var name = GetXmlAttributeValue(parameter.Attribute("Name"));
                var value = GetXmlAttributeValue(parameter.Attribute("Value"));

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    firmParameters[name] = value;
                }
            }

            return firmParameters;
        }
    }
}