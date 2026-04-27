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
        public IActionResult Index(AddClientDto model)
        {
            if (ModelState.IsValid)
            {
                _clientService.AddClient(model);
                return RedirectToAction("Index");
            }

            var vm = new ClientsIndexViewModel
            {
                Clients = _clientService.GetAllClients(),
                NewClient = model
            };
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

        [HttpPut("{id}")]
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
            };
            return View(editClientDto);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchClientDto obj) 
        { 
            return View(_clientService.SearchClient(obj));
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
