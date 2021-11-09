using System;
using System.IO;
using System.Net;
using System.Text.Json;
using Lanchat.Core.Config;

namespace Lanchat.ClientCore
{
    /// <inheritdoc />
    public class NodesDatabase : INodesDatabase
    {
        /// <inheritdoc />
        public string GetLocalNodeInfo()
        {
            return ReadPemFile("localhost");
        }

        /// <inheritdoc />
        public void SaveLocalNodeInfo(string pem)
        {
            var nodeInfo = new NodeInfo
            {
                Id = 0,
                IpAddress = IPAddress.Loopback
            };

            SaveNodeInfo("localhost", nodeInfo);
            SavePemFile("localhost", pem);
        }

        /// <inheritdoc />
        public NodeInfo GetNodeInfo(IPAddress ipAddress)
        {
            var pem = ReadPemFile(ipAddress.ToString());
            var nodeInfo = ReadNodeInfo(ipAddress.ToString());
            nodeInfo.PublicKey = pem;
            return nodeInfo;
        }

        /// <inheritdoc />
        public void SaveNodeInfo(IPAddress ipAddress, NodeInfo nodeInfo)
        {
            SaveNodeInfo(ipAddress.ToString(), nodeInfo);
            SavePemFile(ipAddress.ToString(), nodeInfo.PublicKey);
        }

        private static NodeInfo ReadNodeInfo(string name)
        {
            try
            {
                var json = File.ReadAllText($"{Storage.DatabasePath}/{name}.json");
                return JsonSerializer.Deserialize<NodeInfo>(json);
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptions(e);
                return null;
            }
        }
        
        private static void SaveNodeInfo(string name, NodeInfo nodeInfo)
        {
            try
            {
                var json = JsonSerializer.Serialize(nodeInfo);
                var filePath = $"{Storage.RsaDatabasePath}/{name}.pem";
                Storage.CreateStorageDirectoryIfNotExists();
                Storage.CreateAndSetPermissions(filePath);
                File.WriteAllText(filePath, json);
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptions(e);
            }
        }
        
        private static string ReadPemFile(string name)
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

        private static void SavePemFile(string name, string content)
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
    }
}