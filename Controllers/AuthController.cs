using APIBanco.Domain.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace APIBanco.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpPost]
        //public IActionResult Auth(string username, string password)
        //{
        //    //TODO
        //    //Buscar no banco o usuario e senha correspondentes
        //    //var User = _context.
        //    if (username == "" && password == "")
        //    {
        //        var token = TokenService.GenerateToken(new Model.Conta());
        //        return Ok(token);
        //    }
                
        //    return BadRequest("Usuario ou Senha Invalidos");
        //}
    }
}
