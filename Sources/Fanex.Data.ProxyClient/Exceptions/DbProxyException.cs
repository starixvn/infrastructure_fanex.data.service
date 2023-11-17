namespace Fanex.Data
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    [Serializable]
    public class DbProxyException : Exception
    {
        private static readonly Regex PasswordRegex = new Regex(@"(password|pwd)\s*=\s*(.*?)(;|$)", RegexOptions.IgnoreCase);

        public DbProxyException(string message)
            : base(message)
        {
        }

        public DbProxyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DbProxyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected static string ObscurePassword(string message)
        {
            return PasswordRegex.Replace(message, "Password=xxxx-xxxx-xxxx-xxxx;");
        }
    }
}