using System.ComponentModel.DataAnnotations;

namespace Lanchat.Tests.Mock
{
    public class ModelWithValidationMock
    {
        [Required]
        public string Property { get; set; }
    }
}