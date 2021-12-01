using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Lanchat.Core.Network;

namespace Lanchat.Core.Api
{
    internal class Validation
    {
        private readonly INodeInternal node;

        public Validation(INodeInternal node)
        {
            this.node = node;
        }

        internal bool CheckPreconditions(IApiHandler handler, object data)
        {
            if (!node.Ready && !handler.Privileged)
            {
                Trace.WriteLine($"{node.Id} must be ready to handle this type of data.");
                return false;
            }

            if (handler.Disabled)
            {
                Trace.WriteLine("Handler disabled");
                return false;
            }

            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(
                data,
                new ValidationContext(data),
                results,
                true))
            {
                return true;
            }

            var resultsString = string.Join(", ", results.Select(x => x.ErrorMessage));
            Trace.WriteLine($"Node {node.Id} received invalid json: {resultsString}");
            return false;
        }
    }
}