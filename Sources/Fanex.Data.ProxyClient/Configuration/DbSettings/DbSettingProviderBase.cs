using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public abstract class DbSettingProviderBase
    {
        protected DbSettingProviderBase(string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                throw new ArgumentException(
                    $"{nameof(resourcePath)} is required. We use this directory path to detach Stored Procedure configuration.",
                    nameof(resourcePath));
            }

            ResourcePath = GetFullResourcePath(resourcePath);
        }

        public abstract IDictionary<string, DbSetting> SettingDataSource { get; }

        public string ResourcePath { get; set; }

        protected internal ConnectionStringProviderBase ConnectionStringProvider { get; set; }

        public abstract DbSettingProviderBase InsertSetting(DbSetting setting);

        public abstract DbSettingProviderBase UpdateSetting(DbSetting setting);

        public abstract DbSettingProviderBase DeleteSetting(DbSetting setting);

        public abstract void Start();

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
    }
}