namespace Fanex.Data
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    [Serializable]
    public class DbProxyException : Exception
    {
        private static readonly Regex PasswordRegex1 = new Regex(@"password\s*=\s*(.*?);", RegexOptions.IgnoreCase);
        private static readonly Regex PasswordRegex2 = new Regex(@"password\s*=\s*(.*?)""", RegexOptions.IgnoreCase);
        private static readonly Regex PasswordRegex3 = new Regex(@"pwd\s*=\s*(.*?);", RegexOptions.IgnoreCase);
        private static readonly Regex PasswordRegex4 = new Regex(@"pwd\s*=\s*(.*?)""", RegexOptions.IgnoreCase);

        public DbProxyException(string message)
            : base(message)
        {
        }

        public DbProxyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

   
        protected static string ObscurePassword(string message)
        {
            message = PasswordRegex1.Replace(message, "password=xxxx-xxxx-xxxx-xxxx;");
            message = PasswordRegex2.Replace(message, "password=xxxx-xxxx-xxxx-xxxx\"");
            message = PasswordRegex3.Replace(message, "pwd=xxxx-xxxx-xxxx-xxxx;");
            message = PasswordRegex4.Replace(message, "pwd=xxxx-xxxx-xxxx-xxxx\"");

            return message;
        }
    }
}