using ClientsDtosWebService.Data;
using ClientsDtosWebService.DTOs;
using ClientsDtosWebService.Models;

namespace ClientsDtosWebService.Services
{
    public class ClientService
    {
        private readonly AppClientDbContext _context;

        public ClientService(AppClientDbContext context) 
        {
            _context = context;
        } 
        
        public Client AddClient(AddClientDto obj)
        {
            var client = new Client
            {
                Name = obj.Name,
                Email = obj.Email,
                Password = obj.Password,
            };
            _context.Clients.Add(client);
            _context.SaveChanges();
            return client;
        }

        public List<Client> GetAllClients()
        {
            return _context.Clients.ToList();
        }

        public Client? GetClientById(int id)
        { 
            return _context.Clients.FirstOrDefault(c => c.Id == id);
        }

        public Client? UpdateClient(int id, EditClientDto obj)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return null;
            }
            client.Name = obj.Name;
            client.Email = obj.Email;
            client.Password = obj.Password;
            client.Age = obj.Age;
            _context.SaveChanges();
            return client;
        }

        public List<Client> SearchClient(SearchClientDto obj)
        {
            return _context.Clients.Where(c => c.Name.Contains(obj.Name) && c.Email.Contains(obj.Email)).ToList();
        }
        public bool DeleteClient(int id)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return false;
            }
            _context.Clients.Remove(client);
            _context.SaveChanges();
            return true;
        }
    }
}
