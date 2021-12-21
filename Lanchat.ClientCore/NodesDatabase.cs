using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using Lanchat.Core.Config;
using Lanchat.Core.Json;

namespace Lanchat.ClientCore
{
    /// <inheritdoc />
    public class NodesDatabase : INodesDatabase
    {
        private readonly List<NodeInfo> savedNodes;

        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonIpAddressConverter()
            }
        };

        private static readonly NodesDatabaseContext NodesDatabaseContext = new(JsonSerializerOptions);

        /// <summary>
        ///     Loads database from json file.
        /// </summary>
        /// <remakrs>
        ///     To change file paths use <see cref="Paths"/>.
        /// </remakrs>
        public NodesDatabase()
        {
            try
            {
                var json = File.ReadAllText(Paths.NodesFile);
                savedNodes = JsonSerializer.Deserialize(json, NodesDatabaseContext.ListNodeInfo);
                savedNodes?.ForEach(x => x.PropertyChanged += (_, _) => SaveNodesList());
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptions(e);
                savedNodes = new List<NodeInfo>();
                SaveNodesList();
            }
        }

        /// <inheritdoc />
        public List<INodeInfo> SavedNodes => savedNodes.Cast<INodeInfo>().ToList();

        /// <inheritdoc />
        public string GetLocalNodeInfo()
        {
            return ReadPemFile("localhost");
        }

        /// <inheritdoc />
        public void SaveLocalNodeInfo(string pem)
        {
            SavePemFile("localhost", pem);
        }

        /// <inheritdoc />
        public INodeInfo GetNodeInfo(IPAddress ipAddress)
        {
            return savedNodes.Find(x => Equals(x.IpAddress, ipAddress)) ?? CreateNodeInfo(ipAddress);
        }

        /// <inheritdoc />
        public INodeInfo GetNodeInfo(int id)
        {
            return savedNodes.Find(x => x.Id == id);
        }

        internal static string ReadPemFile(string name)
        {
            try
            {
                return File.ReadAllText($"{Paths.RsaDirectory}/{name}.pem");
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptions(e);
                return null;
            }
        }

        internal static void SavePemFile(string name, string content)
        {
            var filePath = $"{Paths.RsaDirectory}/{name}.pem";
            Storage.SaveFile(content, filePath);
        }

        private NodeInfo CreateNodeInfo(IPAddress ipAddress)
        {
            var nodeInfo = new NodeInfo
            {
                IpAddress = ipAddress,
                Id = savedNodes.Count + 1
            };

            nodeInfo.PropertyChanged += (_, _) => SaveNodesList();
            savedNodes.Add(nodeInfo);
            SaveNodesList();
            return nodeInfo;
        }

        private void SaveNodesList()
        {
            var json = JsonSerializer.Serialize(savedNodes, NodesDatabaseContext.ListNodeInfo);
            Storage.SaveFile(json, Paths.NodesFile);
        }
    }
}