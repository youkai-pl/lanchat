using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Identity;
using Lanchat.Core.NodesDiscovery;

namespace Lanchat.Core.Network
{
    internal static class NodeSetup
    {
        internal static IContainer Setup(
            IStorage storage,
            IConfig config,
            INodesDatabase nodesDatabase,
            ILocalRsa localRsa,
            IP2P network,
            Action<IActivatedEventArgs<INode>> nodeCreated,
            IEnumerable<Type> handlers = null)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(storage)
                .As<IStorage>()
                .SingleInstance();

            builder.RegisterInstance(config)
                .As<IConfig>()
                .SingleInstance();

            builder.RegisterInstance(nodesDatabase)
                .As<INodesDatabase>()
                .SingleInstance();

            builder.RegisterInstance(network)
                .As<IP2P>()
                .SingleInstance();

            builder.RegisterInstance(localRsa)
                .As<ILocalRsa>()
                .SingleInstance();

            builder.RegisterType<NodesExchange>()
                    .As<INodesExchange>()
                    .SingleInstance();

            builder.RegisterType<Node>()
                .As<INode>()
                .OnActivated(nodeCreated)
                .As<INodeInternal>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<User>()
                .As<IUser>()
                .As<IInternalUser>()
                .InstancePerLifetimeScope();

            builder.RegisterType<NodeRsa>()
                .As<INodeRsa>()
                .As<IInternalNodeRsa>()
                .InstancePerLifetimeScope();

            builder.RegisterType<NodeAes>()
                .As<INodeAes>()
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

            builder.RegisterType<Messaging>()
                .As<IMessaging>()
                .As<IInternalMessaging>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FileReceiver>()
                .As<IFileReceiver>()
                .As<IInternalFileReceiver>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FileSender>()
                .As<IFileSender>()
                .As<IInternalFileSender>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Connection>()
                .As<IConnection>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FileTransferOutput>()
            .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(
                    Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Handler"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            handlers?.ForEach(x => builder
                .RegisterType(x)
                .As<IApiHandler>()
                .AsImplementedInterfaces());

            return builder.Build();
        }
    }
}