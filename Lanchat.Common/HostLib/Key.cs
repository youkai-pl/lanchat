namespace Lanchat.Common.HostLib
{
    public class Key
    {
        public Key(string aeskey, string aesvi)
        {
            AesKey = aeskey;
            AesVI = aesvi;
        }

        public string AesKey { get; set; }
        public string AesVI { get; set; }
    }
}