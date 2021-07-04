namespace Lanchat.Core.Encryption
{
    /// <summary>
    ///     Events related to encryption.
    /// </summary>
    public interface INodeRsa
    {
        /// <summary>
        ///     Node public key in PEM format.
        /// </summary>
        string PublicPem { get; }

        /// <summary>
        ///     Result of key validation.
        /// </summary>
        KeyStatus KeyStatus { get; }
    }

    /// <summary>
    ///     Key validation status.
    /// </summary>
    public enum KeyStatus
    {
        /// <summary>
        ///     The key was not previously saved in database.
        /// </summary>
        FreshKey,

        /// <summary>
        ///     The key is different from stored in database
        /// </summary>
        ChangedKey,

        /// <summary>
        ///     Key us same as stored in database.
        /// </summary>
        ValidKey
    }
}