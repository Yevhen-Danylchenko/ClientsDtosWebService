using ClientsDtosWebService.DTOs;

namespace ClientsDtosWebService.Models
{
    public class SearchClientsViewModel
    {
        public SearchClientDto Criteria { get; set; } = new();
        public IEnumerable<Client> Results { get; set; } = new List<Client>();
    }
}
