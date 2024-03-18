using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIBanco.Controllers
{
    public class ContaController : Controller
    {
        [Authorize]
        [HttpPost]
        public IActionResult Add()
        {
            return View();
        }
    }
}
