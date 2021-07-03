namespace Lanchat.Core.Config
{
    /// <summary>
    ///     RSA keys storage.
    /// </summary>
    public interface IRsaDatabase
    {
        /// <summary>
        ///     Get local public and private keys.
        /// </summary>
        /// <returns>PEM file string</returns>
        string GetLocalPem();
        
        /// <summary>
        ///     Save local public and private keys.
        /// </summary>
        /// <param name="pem">PEM file string</param>
        void SaveLocalPem(string pem);
    }
}