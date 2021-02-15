using System;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class StatusUpdateHandler : IApiHandler
    {
        private readonly Node node;

        public StatusUpdateHandler(Node node)
        {
            this.node = node;
        }
        
        public DataTypes DataType { get; } = DataTypes.StatusUpdate;
        public void Handle(object data)
        {
            if (Enum.TryParse<Status>((string)data, out var status)) node.Status = status;
        }
    }
}