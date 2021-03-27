using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Lanchat.Core.NetworkIO
{
    internal class NetworkInput
    {
        private readonly Resolver resolver;
        private string buffer;
        private string currentJson;

        internal NetworkInput(Resolver resolver)
        {
            this.resolver = resolver;

            resolver.Models.AddRange(Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.Namespace == "Lanchat.Core.Models"));
        }

        internal void ProcessReceivedData(object sender, string dataString)
        {
            buffer += dataString;
            var index = buffer.LastIndexOf("}", StringComparison.Ordinal);
            if (index < 0) return;
            currentJson = buffer.Substring(0, index + 1);
            buffer = buffer.Substring(index + 1);

            foreach (var item in currentJson.Replace("}{", "}|{").Split('|'))
                try
                {
                    resolver.Handle(item);
                }

                // Input errors catching.
                catch (Exception ex)
                {
                    if (ex is not JsonException &&
                        ex is not ArgumentNullException &&
                        ex is not InvalidOperationException &&
                        ex is not ArgumentException &&
                        ex is not ValidationException) throw;
                }
        }
    }
}