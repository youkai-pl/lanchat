namespace Lanchat.Core.Models
{
    internal class Wrapper
    {
        public DataTypes Type { get; set; }
        public object Data { get; set; }
    }

    public enum DataTypes
    {
        Message,
        Ping,
        Handshake,
        NodesList,
        KeyInfo
    }
}