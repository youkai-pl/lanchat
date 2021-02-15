using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class PingHandler : IApiHandler
    {
        private readonly Echo echo;

        public PingHandler(Echo echo)
        {
            this.echo = echo;
        }
        
        public DataTypes DataType { get; } = DataTypes.Ping;
        public void Handle(object data)
        {
            echo.HandlePing();
        }
    }
}