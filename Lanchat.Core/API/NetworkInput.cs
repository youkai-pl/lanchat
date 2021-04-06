using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Lanchat.Core.API
{
    internal class NetworkInput
    {
        private readonly Resolver resolver;
        private string buffer;
        private string currentJson;

        internal NetworkInput(Resolver resolver)
        {
            this.resolver = resolver;
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
                    resolver.HandleJson(item);
                }
                catch (JsonException)
                {
                }
                catch (ArgumentNullException)
                {
                }
                catch (InvalidOperationException)
                {
                }
                catch (ArgumentException)
                {
                }
                catch (ValidationException)
                {
                }
        }
    }
}