namespace Lanchat.Common.HostLib
{
    public class Handshake
    {
        public Handshake(string nickname, string publicKey)
        {
            Nickname = nickname;
            PublicKey = publicKey;
        }

        public string Nickname { get; set; }
        public string PublicKey { get; set; }
    }
}
