using System;

namespace Lanchat.Core.NetworkIO
{
    /// <summary>
    ///     Inherit this class for create custom API handler.
    /// </summary>
    /// <typeparam name="T">Type of handled model</typeparam>
    public abstract class ApiHandler<T> : IApiHandler
    {
        /// <summary>
        ///     Type of handled model.
        /// </summary>
        public Type HandledType { get; } = typeof(T);

        /// <summary>
        ///     If handler is privileged it will process data even if node is unready.
        /// </summary>
        public bool Privileged { get; set; }

        /// <summary>
        ///     Handle unconverted object.
        /// </summary>
        /// <param name="data">Unconverted object.</param>
        public void Handle(object data)
        {
            Handle((T) data);
        }

        /// <summary>
        ///     Handle object converted to required type.
        /// </summary>
        /// <param name="data">Converted object.</param>
        protected abstract void Handle(T data);
    }
}