using System.ComponentModel.DataAnnotations;

namespace Lanchat.Tests.Mock.Models
{
    public class ModelWithValidationRequired
    {
        [Required] public string Property { get; set; }
    }
}