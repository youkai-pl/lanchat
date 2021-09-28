using System.ComponentModel.DataAnnotations;

namespace Lanchat.Tests.Mock.Models
{
    public class ModelWithValidationMaxLength
    {
        [MaxLength(5)] public string Property { get; set; }
    }
}