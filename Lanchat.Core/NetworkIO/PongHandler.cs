using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class PongHandler : IApiHandler
    {
        private readonly Echo echo;

        public PongHandler(Echo echo)
        {
            this.echo = echo;
        }
        
        public DataTypes DataType { get; } = DataTypes.Ping;
        public void Handle(object data)
        {
            echo.HandlePong();
        }
    }
}