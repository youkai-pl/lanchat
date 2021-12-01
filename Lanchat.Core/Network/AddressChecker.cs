using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Config;

namespace Lanchat.Core.Network
{
    /// <summary>
    ///     IP addresses tools.
    /// </summary>
    public class AddressChecker
    {
        private readonly IConfig config;
        private readonly INodesDatabase nodesDatabase;
        private readonly List<IPAddress> connections = new();

        internal AddressChecker(IConfig config, INodesDatabase nodesDatabase)
        {
            this.config = config;
            this.nodesDatabase = nodesDatabase;
        }

        internal void CheckAddress(IPAddress ipAddress)
        {
            var savedNode = nodesDatabase.SavedNodes.Find(x => Equals(x.IpAddress, ipAddress));
            if (savedNode is { Blocked: true })
            {
                throw new ArgumentException("Node blocked");
            }

            lock (connections)
            {
                if (connections.Any(x => x.Equals(ipAddress)))
                {
                    throw new ArgumentException($"Already connecting or connected wit {ipAddress}");
                }
            }

            if (GetLocalAddresses().Any(x => x.Equals(ipAddress)) && !config.DebugMode)
            {
                throw new ArgumentException($"{ipAddress} belong to local machine");
            }
        }

        internal void LockAddress(IPAddress ipAddress)
        {
            lock (connections)
            {
                if (connections.Contains(ipAddress))
                {
                    return;
                }

                connections.Add(ipAddress);
            }
        }

        internal void UnlockAddress(IPAddress ipAddress)
        {
            lock (connections)
            {
                connections.Remove(ipAddress);
            }
        }

        /// <summary>
        ///     Get list of host IP addresses.
        /// </summary>
        /// <returns>List of addresses</returns>
        public static IEnumerable<IPAddress> GetLocalAddresses()
        {
            try
            {
                var list = new List<IPAddress>();
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        list.Add(ip);
                    }
                }
                return list;
            }
            catch (SocketException)
            {
                Trace.WriteLine("Cannot get local addresses");
                return new[]
                {
                    IPAddress.Loopback,
                    IPAddress.IPv6Loopback
                };
            }
        }
    }
}