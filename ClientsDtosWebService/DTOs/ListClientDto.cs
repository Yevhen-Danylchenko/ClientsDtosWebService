using Microsoft.Extensions.Primitives;

namespace ClientsDtosWebService.DTOs
{
    public class ListClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
