using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;

[SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
public class DbSettingProvider
{
    private readonly ConcurrentDictionary<string, DbSetting> settingDataSource = new(StringComparer.OrdinalIgnoreCase);

    public DbSettingProvider(string resourcePath)
    {
        if (string.IsNullOrWhiteSpace(resourcePath))
        {
            throw new ArgumentException(
                $"{nameof(resourcePath)} is required. We use this directory path to detach Stored Procedure configuration.",
                nameof(resourcePath));
        }

        ResourcePath = GetFullResourcePath(resourcePath);
    }

    public string ResourcePath { get; set; }

    protected internal ConnectionStringProvider ConnectionStringProvider { get; set; }

    
    public virtual void Restart()
        => Start();

    public virtual DbSetting GetSetting(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key must not be null or empty.", nameof(key));
        }

        if (SettingDataSource == null)
        {
            throw new InvalidOperationException(
                $"No Stored Procedure configuration was detected! You have not invoked the {nameof(Start)} method.");
        }

        if (!SettingDataSource.ContainsKey(key))
        {
            throw new KeyNotFoundException($"There is no Stored Procedure configuration with the key named '{key}' found.");
        }

        var dbSetting = SettingDataSource[key];

        if (dbSetting == null)
        {
            throw new InvalidOperationException($"The '{key}' is pointing to a null DbSetting.");
        }

        return dbSetting.Clone();
    }

    protected static bool IsFolder(string path)
    {
        var fileAttribute = File.GetAttributes(path);

        return (fileAttribute & FileAttributes.Directory) == FileAttributes.Directory;
    }

    protected static string GetFullResourcePath(string resourcePath)
    {
        var fullResourcePath = resourcePath;

        if (!Path.IsPathRooted(fullResourcePath))
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;

            fullResourcePath = fullResourcePath.TrimStart('~', '\\', '/');
            fullResourcePath = Path.Combine(rootPath, fullResourcePath);
        }

        EnsureResourceExist(fullResourcePath);

        return fullResourcePath;
    }

    protected static void EnsureResourceExist(string resourcePath)
    {
        if (Directory.Exists(resourcePath))
        {
            if (Directory.GetFiles(resourcePath).Length == 0)
            {
                throw new FileNotFoundException($"'{resourcePath}' does not contain any files.");
            }

            return;
        }

        if (File.Exists(resourcePath))
        {
            return;
        }

        throw new FileNotFoundException($"'{resourcePath}' was not found.");
    }

       
    public virtual IDictionary<string, DbSetting> SettingDataSource => settingDataSource;

    public virtual DbSettingProvider DeleteSetting(DbSetting setting)
    {
        settingDataSource.TryRemove(setting.Key, out _);

        return this;
    }

    public virtual DbSettingProvider InsertSetting(DbSetting setting)
    {
        settingDataSource.TryAdd(setting.Key, setting);

        return this;
    }

    public virtual DbSettingProvider UpdateSetting(DbSetting setting)
    {
        if (settingDataSource.TryGetValue(setting.Key, out var oldSetting))
        {
            settingDataSource.TryUpdate(setting.Key, setting, oldSetting);
        }

        return this;
    }

    public void Start()
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
            resourceFiles = [resourcePath];
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
            using var streamReader = CreateStreamReader(filePath);
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
                        // ReSharper disable once UnusedParameter.Local
                        (k, _) => dbSetting);
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

    public void UpdateConnectionString(Dictionary<string, ConnectionConfiguration> connectionMaps)
    {
        var settingKeys = SettingDataSource.Keys.ToArray();
        foreach (var settingKey in settingKeys)
        {
            var oldSetting = SettingDataSource[settingKey];
            if (!connectionMaps.TryGetValue(oldSetting.DbName, out var connectionConfiguration))
            {
                continue;
            }

            var newSetting = new DbSetting(
                settingKey,
                oldSetting.Provider,
                oldSetting.DbName,
                connectionConfiguration.ConnectionString,
                oldSetting.SpName,
                oldSetting.CommandTimeout,
                oldSetting.ColumnMaps,
                oldSetting.FirmParameters);

            SettingDataSource[settingKey] = newSetting;
        }
    }
}