using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Usuarios.API.Aplicacao.Interface;
using Usuarios.API.Model;
using Usuarios.API.Request;
using Usuarios.API.Response;

namespace Usuarios.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        //private VagaContext _context;
        //private IMapper _mapper;
        //   ILogger logger,

        public UsuarioController(
            IUsuarioService usuarioService
            //VagaContext context, IMapper mapper
            )
        {
            _usuarioService = usuarioService;
            //_context = context;
            //_mapper = mapper;
        }

        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        /// <param name="usuarioDto">Dados do novo usuário (Nome, Email, Senha, Telefone).</param>
        /// <returns>A resposta padronizada com o status do cadastro.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resposta<dynamic>>> CadastrarUsuario([FromBody] UsuarioRequest usuarioRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var usuario = new Usuario
            {
                Nome = usuarioRequest.Nome,
                Email = usuarioRequest.Email,
                Senha = usuarioRequest.Senha,
                Telefone = usuarioRequest.Telefone,
            };

            var resultado = await _usuarioService.CadastrarUsuarioAsync(usuario);

            if (resultado.Sucesso && resultado.Status == 201)
            {
                return CreatedAtAction(nameof(CadastrarUsuario), resultado);
            }
            else if (!resultado.Sucesso && resultado.Status == 400)
            {
                return BadRequest(resultado);
            }
            else 
            {
                //_logger.LogError("Erro inesperado durante o cadastro de usuário: {Titulo}", resultado.Titulo);
                return StatusCode(StatusCodes.Status500InternalServerError, resultado);
            }
        }
    }
}
/*
    /// <summary>
    /// Lista uma pagina de vagas conforme os parametros
    /// </summary>
    /// <param name="skip">A partir d </param>
    /// <param name="take">Quantos d </param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a listagem de vagas seja feita com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IEnumerable<UsuarioResponse> ListaVagas([FromQuery] int skip = 0, [FromQuery] int take = 25)
    {
        return _mapper.Map<List<UsuarioResponse>>(_context.Vagas.Skip(skip).Take(take));
    }

    /// <summary>
    /// Busca uma vaga por Identificador (id)
    /// </summary>
    /// <param name="id"> Id da vaga a ser alterada </param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a vagas exista e tenha sido encontrada com sucesso</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult BuscaVagaPorId(int id)
    {
        Vaga? vaga = _context.Vagas.FirstOrDefault(v => v.Id == id);

        if (vaga == null)
        {
            return NotFound();
        }
        //Alterar para Map
        var vagaResponse = _mapper.Map<UsuarioRequest>(vaga);

        return Ok(vagaResponse);
    }
    /// <summary>
    /// Atualiza uma vaga com PUT por Identificador (id) e parametros (dados) com as novas informações da vaga a ser alterada.
    /// </summary>
    /// <param name="id"> Id da vaga a ser buscada </param>
    /// <param name="vagaRequest"> Objeto com novas informações para atualização no banco de dados </param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a vaga tenha sido toda atualizada com sucesso</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizaVagaCompleta(int id, [FromBody] UsuarioRequest usuarioRequest)
    {
        Vaga? vaga = _context.Vagas.FirstOrDefault(vaga => vaga.Id == id);

        if (vaga == null)
        {
            return NotFound();
        }
        _mapper.Map(usuarioRequest, vaga);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Atualiza uma vaga com PATCH por Identificador (id) apenas parametros (dados) necessarior com as novas informações da vaga.
    /// </summary>
    /// <param name="id"> Id da vaga a ser buscada </param>
    /// <param name="patch"> Partes e suas propriedades que serão atualizadas no banco de dados do tipo JsonPatchDocument<VagaRequest></param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a vaga tenha sido toda atualizada com sucesso</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizarVaga(int id, [FromBody] JsonPatchDocument<UsuarioRequest> patch)
    {
        Usuario? vaga = _context.Vagas.FirstOrDefault(vaga => vaga.Id == id);
        if (vaga == null)
        {
            return NotFound();
        }
        var vagaParaAtualizar = _mapper.Map<UsuarioRequest>(vaga);
        patch.ApplyTo(vagaParaAtualizar, ModelState);


        if (!TryValidateModel(vagaParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(vagaParaAtualizar, vaga);
        _context.SaveChanges();

        return NoContent();
    }
    /// <summary>
    /// Deleta uma vaga por identificador 
    /// </summary>
    /// <param name="id"> Identificador (id) para encontrar a vaga que será deletada </param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a remoção seja feita com sucesso</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult DeletarVaga(int id)
    {
        try
        {
            var vaga = _context.Vagas.FirstOrDefault(vaga => vaga.Id == id);
            if (vaga == null)
            {
                return NotFound();
            }

            _context.Remove(vaga);
            _context.SaveChanges();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao remover vaga: {ex.Message}");
        }
    }

    /// <summary>
    /// Insere um Lote Mock de vagas fake para teste do banco de dados
    /// </summary>
    /// <param name="vagaRequest"> Identificador do Lote</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost("{lote}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult InserirLote(int lote)
    {
        try
        {
            List<Usuario> lista = new List<Usuario>();
            for (int i = 0; i < 50; i++)
            {
                Usuario vagaAberta = new Usuario
                {
                    Nome = "vaga" + i,
                    Email = "A vaga é para " + i,
                    Senha = "remoto"
                };
                lista.Add(vagaAberta);
            }
            _context.Vagas.AddRange(lista);
            _context.SaveChanges();

            return Ok(new { message = $"{lote} vagas inseridas com sucesso." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao inserir vagas: {ex.Message}");
        }
    }
  }
}*/
