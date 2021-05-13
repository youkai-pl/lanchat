using Autofac;
using Lanchat.Core.Api;
using Lanchat.Core.Chat.Handlers;
using Lanchat.Core.Encryption.Handlers;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network.Handlers;

namespace Lanchat.Core.Network
{
    internal static class HandlersSetup
    {
        internal static void RegisterHandlers(IResolver resolver, IContainer container)
        {
            resolver.RegisterHandler(container.Resolve<HandshakeHandler>());
            resolver.RegisterHandler(container.Resolve<KeyInfoHandler>());
            resolver.RegisterHandler(container.Resolve<ConnectionControlHandler>());
            resolver.RegisterHandler(container.Resolve<UserStatusUpdateHandler>());
            resolver.RegisterHandler(container.Resolve<NicknameUpdateHandler>());
            resolver.RegisterHandler(container.Resolve<MessageHandler>());
            resolver.RegisterHandler(container.Resolve<FilePartHandler>());
            resolver.RegisterHandler(container.Resolve<FileReceiveRequestHandler>());
            resolver.RegisterHandler(container.Resolve<FileTransferControlHandler>());
        }
    }
}