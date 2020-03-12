using System;

namespace Lanchat.Common.NetworkLib.Events.Args
{
    /// <summary>
    /// Host started.
    /// </summary>
    public class HostStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Host listening port.
        /// </summary>
        public int Port { get; set; }
    }
}
