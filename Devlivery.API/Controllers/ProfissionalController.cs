using Devlivery.API.Models;
using Devlivery.API.Request;
using Devlivery.API.Request.RequestHttp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devlivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfissionalController : Controller
    {
        [HttpGet]
        //[ProducesResponseType(StatusCodes.Status200Created)]
        public IActionResult ObterProfissionais()
        {
            return Ok("Teste 0");
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionarProfissional([FromBody] UsuarioRequestHttp usuarioRequestHttp)
        {
            Console.WriteLine(usuarioRequestHttp.Nome);
            return Ok("Teste" + usuarioRequestHttp.Email);
        }
    }
}