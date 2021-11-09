using System;
using System.IO;
using System.Net;
using System.Text.Json;
using Lanchat.Core.Config;
using Lanchat.Core.Json;

namespace Lanchat.ClientCore
{
    /// <inheritdoc />
    public class NodesDatabase : INodesDatabase
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            Converters = { new IpAddressConverter() }
        };

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
        public NodeInfo GetNodeInfo(IPAddress ipAddress)
        {
            var pem = ReadPemFile(ipAddress.ToString());
            var nodeInfo = ReadNodeInfo(ipAddress.ToString()) ?? new NodeInfo
            {
                Id = Directory.GetFiles(Storage.DatabasePath).Length + 1
            };

            nodeInfo.PublicKey = pem;
            return nodeInfo;
        }

        /// <inheritdoc />
        public void UpdateNodeNickname(IPAddress ipAddress, string nickname)
        {
            var nodeInfo = GetNodeInfo(ipAddress);
            nodeInfo.Nickname = nickname;
            SaveNodeInfo(ipAddress.ToString(), nodeInfo);

            if (nodeInfo.PublicKey != null)
            {
                SavePemFile(ipAddress.ToString(), nodeInfo.PublicKey);
            }
        }

        /// <inheritdoc />
        public void UpdateNodePublicKey(IPAddress ipAddress, string publicKey)
        {
            SavePemFile(ipAddress.ToString(), publicKey);
        }

        private NodeInfo ReadNodeInfo(string name)
        {
            try
            {
                var json = File.ReadAllText($"{Storage.DatabasePath}/{name}.json");
                return JsonSerializer.Deserialize<NodeInfo>(json, jsonSerializerOptions);
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptions(e);
                return null;
            }
        }

        private void SaveNodeInfo(string name, NodeInfo nodeInfo)
        {
            try
            {
                var json = JsonSerializer.Serialize(nodeInfo, jsonSerializerOptions);
                var filePath = $"{Storage.DatabasePath}/{name}.json";
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