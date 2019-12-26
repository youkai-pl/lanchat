namespace Lanchat.Common.HostLib.Types
{
    internal class Key
    {
        internal Key(string aeskey, string aesiv)
        {
            AesKey = aeskey;
            AesIV = aesiv;
        }

        internal string AesIV { get; set; }
        internal string AesKey { get; set; }
    }
}