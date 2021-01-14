using System;

namespace Lanchat.Core.Network
{
    public static class Common
    {
        public static string TruncateAndValidate(string value, int maxLength)
        {
            value = value.Trim();
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException();
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }
    }
}