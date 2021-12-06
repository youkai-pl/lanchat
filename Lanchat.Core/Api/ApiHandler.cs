using System;

namespace Lanchat.Core.Api
{
    /// <summary>
    ///     Base class for creating custom types API handlers.
    /// </summary>
    /// <remarks>
    ///     Custom API handlers can be used to create features not present in standard Lanchat.Core but using its architecture.
    /// </remarks>
    /// <example>
    /// <code>
    ///     public class PixelHandler : ApiHandler&lt;Pixel&gt;
    ///     {
    ///         protected override void Handle(Pixel pixel)
    ///         {
    ///             Main.Canvas.AddPixel(pixel);
    ///         }
    ///     }
    /// </code>
    ///     The cxample comes from <see href="https://github.com/tof4/Lanpaint/">Lanpaint</see>.
    /// </example>
    /// <typeparam name="T">Data model class.</typeparam>
    public abstract class ApiHandler<T> : IApiHandler
    {
        /// <summary>
        ///     Type of handled model.
        /// </summary>
        public Type HandledType { get; } = typeof(T);

        /// <summary>
        ///     Priviliged handlers will be called event if Node is not ready.
        /// </summary>
        /// <remarks>
        ///     This property is set for handlers that are active before the end of the connection process.
        ///     For example Handshake and KeyInfo are set as privileged;
        /// </remarks>
        public bool Privileged { get; protected init; }

        /// <summary>
        ///     If set to true Resolver will ignore incomming data for specified handler.
        /// </summary>
        public bool Disabled { get; protected set; }

        /// <summary>
        ///     Internal handler method. Should be used only for tests.
        /// </summary>
        /// <remarks>
        ///     For some reason this have to be public. Please use Handle(T data) instead.
        /// </remarks>
        /// <param name="data">Model of unkown type.</param>
        public void Handle(object data) => Handle((T)data);

        /// <summary>
        ///     Handle received data.
        /// </summary>
        /// <remakrs>
        ///     Override this method to handle incomming data of specified type.
        /// </remakrs>
        /// <param name="data">Model.</param>
        protected abstract void Handle(T data);
    }

    internal interface IApiHandler
    {
        Type HandledType { get; }

        bool Privileged { get; }

        bool Disabled { get; }

        void Handle(object data);
    }
}