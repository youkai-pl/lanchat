namespace Lanchat.Core.NetworkIO
{
    internal interface INetworkOutput
    {
        void SendUserData(object content);
        void SendSystemData(object content);
    }
}