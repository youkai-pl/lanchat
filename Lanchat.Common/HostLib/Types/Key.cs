using Newtonsoft.Json;

namespace Lanchat.Common.HostLib.Types
{
    internal class Key
    {
        [JsonConstructor]
        internal Key(string aeskey, string aesiv)
        {
            AesKey = aeskey;
            AesIV = aesiv;
        }

        [JsonProperty]
        internal string AesIV { get; set; }

        [JsonProperty]
        internal string AesKey { get; set; }
    }
}