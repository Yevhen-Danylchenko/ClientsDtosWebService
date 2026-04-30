using ClientsDtosWebService.Models;
using ClientsDtosWebService.Services;
using ClientsDtosWebService.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ClientsDtosWebService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClientService _clientService;
        private readonly AzureTableService _azureTableService;

        public HomeController(ClientService clientService, AzureTableService azureTableService)
        {
            _clientService = clientService;
            _azureTableService = azureTableService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new ClientsIndexViewModel
            {
                Clients = _clientService.GetAllClients(),
                NewClient = new AddClientDto()
            };

            // Додатково отримуємо клієнтів з Azure Table
            var azureClients = await _azureTableService.GetAllAsync<Client>();
            ViewBag.AzureClients = azureClients;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ClientsIndexViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Зберігаємо у локальну БД
                var client = _clientService.AddClient(vm.NewClient);

                // Зберігаємо також у Azure Table
                var azureEntity = new Client
                {
                    PartitionKey = "Clients",
                    RowKey = client.Id.ToString(),
                    Name = client.Name,
                    Email = client.Email
                };
                await _azureTableService.AddAsync(azureEntity);

                return RedirectToAction("Index");
            }

            vm.Clients = _clientService.GetAllClients();
            return View(vm);
        }

        public IActionResult Create()
        {
            return View(new ClientFormModel());
        }

        [HttpPost]
        public IActionResult Create(ClientFormModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new AddClientDto
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password
                };

                _clientService.AddClient(dto);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            var client = _clientService.GetClientById(id);
            if (client == null)
            {
                return NotFound();
            }

            var editClientDto = new EditClientDto
            {
                Name = client.Name,
                Email = client.Email,
                Password = client.Password,
                Age = client.Age
            };

            return View(editClientDto);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, EditClientDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _clientService.UpdateClient(id, model);
            if (client == null)
            {
                return NotFound();
            }

            // Оновлюємо також у Azure Table
            var azureEntity = new Client
            {
                PartitionKey = "Clients",
                RowKey = id.ToString(),
                Name = client.Name,
                Email = client.Email
            };
            await _azureTableService.UpdateAsync(azureEntity);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            _clientService.DeleteClient(id);

            // Видаляємо також з Azure Table
            await _azureTableService.DeleteAsync("Clients", id.ToString());

            return RedirectToAction("Index");
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchClientDto obj)
        {
            var results = _clientService.SearchClients(obj);
            return View(results);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

