using System.Reflection;
using Autofac;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Chat.Models;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer;

namespace Lanchat.Core.Network
{
    internal static class NodeSetup
    {
        internal static IContainer Setup(IConfig config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(config).As<IConfig>().SingleInstance();
            builder.RegisterType<Node>()
                .As<Node>()
                .As<INode>()
                .As<INodeInternal>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<PublicKeyEncryption>()
                .As<IPublicKeyEncryption>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SymmetricEncryption>()
                .As<ISymmetricEncryption>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ModelEncryption>()
                .As<IModelEncryption>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Output>()
                .As<IOutput>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Input>()
                .As<IInput>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Resolver>()
                .As<IResolver>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Storage>()
                .As<IStorage>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Messaging>()
                .As<IMessaging>()
                .As<IInternalMessaging>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FileReceiver>()
                .As<IInternalFileReceiver>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FileSender>()
                .As<IFileSender>()
                .As<IInternalFileSender>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HandshakeSender>().InstancePerLifetimeScope();
            builder.RegisterType<Connection>().InstancePerLifetimeScope();
            builder.RegisterType<FileTransferOutput>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(
                    Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Handler") && t.Name != "NodesListHandler")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}