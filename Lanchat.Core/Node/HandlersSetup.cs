using Lanchat.Core.Api;
using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Chat;
using Lanchat.Core.FileTransfer;

namespace Lanchat.Core.Node
{
    internal static class HandlersSetup
    {
        internal static void RegisterHandlers(IResolver resolver, LocalNode node)
        {
            resolver.RegisterHandler(new HandshakeHandler(node.PublicKeyEncryption, node.SymmetricEncryption, node.Output, node));
            resolver.RegisterHandler(new KeyInfoHandler(node.SymmetricEncryption, node));
            resolver.RegisterHandler(new ConnectionControlHandler(node.NetworkElement));
            resolver.RegisterHandler(new StatusUpdateHandler(node));
            resolver.RegisterHandler(new NicknameUpdateHandler(node));
            resolver.RegisterHandler(new MessageHandler(node.Messaging));
            resolver.RegisterHandler(new FilePartHandler(node.FileReceiver));
            resolver.RegisterHandler(new FileTransferControlHandler(node.FileReceiver, node.FileSender));
        }
    }
}