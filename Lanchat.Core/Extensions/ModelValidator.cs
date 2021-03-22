using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Lanchat.Core.Extensions
{
    internal static class ModelValidator
    {
        internal static bool Validate(object data)
        {
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(data, new ValidationContext(data), results, true)) return true;
            foreach (var e in results) Trace.WriteLine($"Received invalid data: {e}");
            return false;
        }
    }
}