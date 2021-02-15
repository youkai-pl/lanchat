using System;
using System.Collections.Generic;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core
{
    public class Echo : IApiHandler
    {
        private readonly INetworkOutput networkOutput;
        private DateTime? pingSendTime;

        internal Echo(INetworkOutput networkOutput)
        {
            this.networkOutput = networkOutput;
        }

        /// <summary>
        ///     Last ping value.
        /// </summary>
        public TimeSpan? LastPing { get; internal set; }

        /// <summary>
        ///     Ping pong.
        /// </summary>
        public event EventHandler<TimeSpan?> PongReceived;

        /// <summary>
        ///     Send ping.
        /// </summary>
        public void SendPing()
        {
            pingSendTime = DateTime.Now;
            networkOutput.SendUserData(DataTypes.Ping);
        }

        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[] {DataTypes.Ping, DataTypes.Pong};

        public void Handle(DataTypes type, string data)
        {
            if (type == DataTypes.Ping)
            {
                networkOutput.SendUserData(DataTypes.Pong);
                return;
            }

            if (type == DataTypes.Pong)
            {
                if (pingSendTime == null) return;
                LastPing = DateTime.Now - pingSendTime;
                pingSendTime = null;
                PongReceived?.Invoke(this, LastPing);
            }
        }
    }
}