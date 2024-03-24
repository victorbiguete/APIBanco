using APIBanco.DTOs;
using APIBanco.Model;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APIBanco.Controllers
{
    public class ClienteController : Controller
    {
        private readonly UserManager<Cliente> _userManager;
        private readonly IMapper _mapper;

        public ClienteController(UserManager<Cliente> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(ClienteDTO clienteDTO)
        {

            return View();
        }
    }
}
