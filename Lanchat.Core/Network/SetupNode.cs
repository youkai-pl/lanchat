using System.Reflection;
using Autofac;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer;

namespace Lanchat.Core.Network
{
    internal static class SetupNode
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
            
            builder.RegisterType<PublicKeyEncryption>().As<IPublicKeyEncryption>().InstancePerLifetimeScope();
            builder.RegisterType<SymmetricEncryption>().As<ISymmetricEncryption>().InstancePerLifetimeScope();
            builder.RegisterType<ModelEncryption>().As<IModelEncryption>().InstancePerLifetimeScope();
            builder.RegisterType<Output>().As<IOutput>().InstancePerLifetimeScope();
            builder.RegisterType<Input>().As<IInput>().InstancePerLifetimeScope();
            builder.RegisterType<Resolver>().As<IResolver>().InstancePerLifetimeScope();
            builder.RegisterType<Storage>().As<IStorage>().InstancePerLifetimeScope();
            builder.RegisterType<HandshakeSender>().InstancePerLifetimeScope();
            builder.RegisterType<Connection>().InstancePerLifetimeScope();
            builder.RegisterType<Messaging>().InstancePerLifetimeScope();
            builder.RegisterType<FileTransferOutput>().InstancePerLifetimeScope();
            builder.RegisterType<FileReceiver>().InstancePerLifetimeScope();
            builder.RegisterType<FileSender>().InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(
                    Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Handler") && t.Name != "NodesListHandler")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            return builder.Build();
        }
    }
}