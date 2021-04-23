namespace Lanchat.Core.Api
{
    public interface IResolver
    {
        /// <summary>
        ///     Add data handler for specific model type.
        /// </summary>
        /// <param name="apiHandler">ApiHandler object.</param>
        void RegisterHandler(IApiHandler apiHandler);
    }
}