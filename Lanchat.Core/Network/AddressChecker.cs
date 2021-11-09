using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Config;

namespace Lanchat.Core.Network
{
    internal class AddressChecker
    {
        private readonly IConfig config;
        private readonly List<IPAddress> connections = new();

        internal AddressChecker(IConfig config)
        {
            this.config = config;
        }

        internal void CheckAddress(IPAddress ipAddress)
        {
            if (config.BlockedAddresses.Contains(ipAddress))
            {
                throw new ArgumentException("Node blocked");
            }

            lock (connections)
            {
                if (connections.Any(x => x.Equals(ipAddress)))
                {
                    throw new ArgumentException("Already connecting or connected");
                }
            }

            if (GetLocalAddresses().Any(x => x.Equals(ipAddress)) && !config.DebugMode)
            {
                throw new ArgumentException("Address belong to local machine");
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

        private static IEnumerable<IPAddress> GetLocalAddresses()
        {
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            }
            catch (SocketException)
            {
                Trace.WriteLine("Cannot get local addresses.");
                return new[]
                {
                    IPAddress.Loopback,
                    IPAddress.IPv6Loopback
                };
            }
        }
    }
}