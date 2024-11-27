using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Fanex.Data.Models;

public class SimpleClass
{
    [JsonExtensionData]
    public IDictionary<string, JToken> UnknownProperty;
}