namespace Fanex.Data;

using System;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Extensions;

public static class Cryptography
{
#pragma warning disable SYSLIB0021
    private static readonly DESCryptoServiceProvider CryptoServiceProvider = new();
#pragma warning restore SYSLIB0021
    private static readonly byte[] RgbKey = [0x21, 0x63, 0x54, 0x5a, 0x91, 0xac, 110, 0xcc];
    private static readonly byte[] RgbIv = "&％#@?,:*!~^."u8.ToArray();

    public static string EncryptPassword(string password)
    {
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }

        using var encryptor = CryptoServiceProvider.CreateEncryptor(RgbKey, RgbIv);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        var plainPasswordInBytes = Encoding.UTF8.GetBytes(password);

        cryptoStream.Write(plainPasswordInBytes, 0, plainPasswordInBytes.Length);
        cryptoStream.FlushFinalBlock();

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    private static string DecryptPassword(string encryptedPassword)
    {
        if (encryptedPassword == null)
        {
            throw new ArgumentNullException(nameof(encryptedPassword));
        }

        var standardizedEncryptedPassword = StandardizeEncryptedPasswordLength(encryptedPassword);
        var encryptedPasswordInBytes = Convert.FromBase64String(standardizedEncryptedPassword);

        using var memoryStream = new MemoryStream(encryptedPasswordInBytes);
        using var decryptor = CryptoServiceProvider.CreateDecryptor(RgbKey, RgbIv);
        using var streamReader = new StreamReader(new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read));
        return streamReader.ReadToEnd().Trim();
    }

    public static string DecryptConnectionString(string connectionString)
    {
        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        var connectionStringBuilder = new DbConnectionStringBuilder
        {
            ConnectionString = connectionString
        };
        var encryptedPassword = connectionStringBuilder.GetPassword();

        if (!string.IsNullOrEmpty(encryptedPassword))
        {
            var decryptedPassword = DecryptPassword(encryptedPassword);

            connectionStringBuilder.SetPassword(decryptedPassword);

            return connectionStringBuilder.ToString();
        }

        return connectionString;
    }

    /// <summary>
    /// Truncate the encrypted password to make it meet the required length of <see cref="Convert.FromBase64String(string)"/> method.
    /// </summary>
    private static string StandardizeEncryptedPasswordLength(string encryptedPassword)
    {
        var result = string.Empty;

        if (!string.IsNullOrWhiteSpace(encryptedPassword))
        {
            var allowedLength = encryptedPassword.Length >> 2 << 2;

            result = encryptedPassword.Substring(0, allowedLength);
        }

        return result;
    }
}