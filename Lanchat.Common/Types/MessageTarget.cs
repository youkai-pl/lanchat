namespace Lanchat.Common.Types
{
    /// <summary>
    /// Message target.
    /// </summary>
    public enum MessageTarget
    {
        /// <summary>
        /// For all nodes.
        /// </summary>
        Broadcast,
        
        /// <summary>
        /// For group of nodes.
        /// </summary>
        Group,
        
        /// <summary>
        /// For specified node.
        /// </summary>
        Private
    }
}
