namespace Lanchat.Core.Models
{
    internal class Wrapper
    {
        internal DataTypes Type { get; set; }
        internal object Data { get; set; }
    }

    internal enum DataTypes
    {
        Message,
        Ping
    }
}