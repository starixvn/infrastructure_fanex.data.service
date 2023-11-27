using System;
using System.Collections.Generic;

namespace Fanex.Data.Models
{
    internal class RemoveCacheResult
    {
        public string ServerName { get; set; }  = string.Empty;

        public Dictionary<string, DateTimeOffset> ServerStatus { get; set; } = new Dictionary<string, DateTimeOffset>();
    }
}
