using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Venta.WebApi.Middleware.Seguridad;
using Venta.WebApi.Models;

namespace Venta.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var token = TokenServices.CreateToken(model);
            return Ok(token);
        }
    }
}
