namespace Lanchat.Common.HostLib.Types
{
    public class Key
    {
        public Key(string aeskey, string aesiv)
        {
            AesKey = aeskey;
            AesIV = aesiv;
        }

        public string AesKey { get; set; }
        public string AesIV { get; set; }
    }
}