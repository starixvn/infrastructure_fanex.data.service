using System.Text.RegularExpressions;

namespace Fanex.Data.Exceptions;

internal static class PasswordObfuscator
{
    private const string Pattern = @"(password|pwd)\s*=\s*(.*?)(;|""|')";
    
    internal static string Obfuscate(string input)
    {
        input = Regex.Replace(input,
            Pattern,
            "$1=xxxx-xxxx-xxxx-xxxx$3",
            RegexOptions.IgnoreCase | RegexOptions.Multiline);
        return input;
    }
}