using System;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class Echo
    {
        private readonly INetworkOutput networkOutput;
        private DateTime? pingSendTime;

        /// <summary>
        ///     Last ping value.
        /// </summary>
        public TimeSpan? LastPing { get; internal set; }  
        
        /// <summary>
        ///     Ping pong.
        /// </summary>
        public event EventHandler<TimeSpan?> PongReceived;
        
        internal Echo(INetworkOutput networkOutput)
        {
            this.networkOutput = networkOutput;
        }

        /// <summary>
        ///     Send ping.
        /// </summary>
        public void SendPing()
        {
            pingSendTime = DateTime.Now;
            networkOutput.SendData(DataTypes.Ping);
        }

        internal void HandlePing()
        {
            networkOutput.SendData(DataTypes.Pong);
        }

        internal void HandlePong()
        {
            if (pingSendTime == null) return;
            LastPing = DateTime.Now - pingSendTime;
            pingSendTime = null;
            PongReceived?.Invoke(this, LastPing);
        }
    }
}