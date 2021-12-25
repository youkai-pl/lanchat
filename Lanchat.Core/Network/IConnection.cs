namespace Lanchat.Core.Network
{
    internal interface IConnection
    {
        bool HandshakeReceived { get; set; }
        void Initialize();
        void SendHandshake();
    }
}