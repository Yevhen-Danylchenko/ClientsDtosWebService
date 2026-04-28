using ClientsDtosWebService.Models;
using ClientsDtosWebService.Services;
using ClientsDtosWebService.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClientsDtosWebService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClientService _clientService;

        public HomeController(ClientService clientService)
        {
            _clientService = clientService;
        }

        public IActionResult Index()
        {
            var vm = new ClientsIndexViewModel
            {
                Clients = _clientService.GetAllClients(),
                NewClient = new AddClientDto()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(ClientsIndexViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _clientService.AddClient(vm.NewClient);
                return RedirectToAction("Index");
            }

            vm.Clients = _clientService.GetAllClients();
            return View(vm);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ClientFormModel model)
        {
            if (ModelState.IsValid)
            {
                var addClientDto = new AddClientDto
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password
                };
                _clientService.AddClient(addClientDto);
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
        public IActionResult Edit(int id, EditClientDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _clientService.GetClientById(id);
            if (client == null)
            {
                return NotFound();
            }

            // Оновлюємо дані клієнта
            client.Name = model.Name;
            client.Email = model.Email;
            client.Password = model.Password;
            client.Age = model.Age;

            // Викликаємо сервіс для збереження змін
            _clientService.UpdateClient(id, model);

            return RedirectToAction("Index");
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchClientDto obj)
        {
            var results = _clientService.SearchClients(obj);
            return View(results);
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            _clientService.DeleteClient(id);
            return RedirectToAction("Index");
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
