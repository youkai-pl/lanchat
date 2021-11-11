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
        
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            WriteIndented = true,
            Converters = { new IpAddressConverter() }
        };

        /// <summary>
        ///     Initialize nodes database.
        /// </summary>
        public NodesDatabase()
        {
            try
            {
                var json = File.ReadAllText($"{Storage.DataPath}/nodes.json");
                savedNodes = JsonSerializer.Deserialize<List<NodeInfo>>(json, jsonSerializerOptions);
                savedNodes?.ForEach(x => x.PropertyChanged += (_, _) => { SaveNodesList(); });
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
            var nodeInfo = savedNodes.FirstOrDefault(x => Equals(x.IpAddress, ipAddress))
                           ?? CreateNodeInfo(ipAddress);

            return nodeInfo;
        }

        internal static string ReadPemFile(string name)
        {
            try
            {
                return File.ReadAllText($"{Storage.RsaDatabasePath}/{name}.pem");
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptions(e);
                return null;
            }
        }

        internal static void SavePemFile(string name, string content)
        {
            try
            {
                var filePath = $"{Storage.RsaDatabasePath}/{name}.pem";
                Storage.CreateStorageDirectoryIfNotExists();
                Storage.CreateAndSetPermissions(filePath);
                File.WriteAllText(filePath, content);
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptions(e);
            }
        }

        private NodeInfo CreateNodeInfo(IPAddress ipAddress)
        {
            var nodeInfo = new NodeInfo
            {
                IpAddress = ipAddress,
                Id = savedNodes.Count + 1
            };

            nodeInfo.PropertyChanged += (_, _) => { SaveNodesList(); };
            savedNodes.Add(nodeInfo);
            SaveNodesList();
            return nodeInfo;
        }

        private void SaveNodesList()
        {
            try
            {
                var json = JsonSerializer.Serialize(savedNodes, jsonSerializerOptions);
                var filePath = $"{Storage.DataPath}/nodes.json";
                Storage.CreateStorageDirectoryIfNotExists();
                Storage.CreateAndSetPermissions(filePath);
                File.WriteAllText(filePath, json);
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptions(e);
            }
        }
    }
}