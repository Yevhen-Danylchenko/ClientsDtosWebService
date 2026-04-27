using ClientsDtosWebService.DTOs;

namespace ClientsDtosWebService.Models
{
    public class ClientsIndexViewModel
    {
        public IEnumerable<Client> Clients { get; set; } = new List<Client>();
        public AddClientDto NewClient { get; set; } = new AddClientDto();
    }
}
