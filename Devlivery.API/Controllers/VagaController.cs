using AutoMapper;
using Devlivery.API.Data;
using Devlivery.API.Models;
using Devlivery.API.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Devlivery.API.Response;

namespace Devlivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VagaController : ControllerBase
    {
        private VagaContext _context;
        private IMapper _mapper;
        public VagaController(VagaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona uma vaga ao banco de dados
        /// </summary>
        /// <param name="vagaRequest">Objeto com os campos necessários para criação de uma vaga </param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionaVaga([FromBody] VagaRequest vagaRequest)
        {
            //if(!string.IsNullOrEmpty(vagaRequest.Titulo) && !string.IsNullOrEmpty(vagaRequest.Tipo)){            }
            //Console.WriteLine(vagaRequest.Titulo);
            Vaga vaga = _mapper.Map<Vaga>(vagaRequest);
            _context.Vagas.Add(vaga);
            _context.SaveChanges();

            return CreatedAtAction(nameof(BuscaVagaPorId),
                new { id = vaga.Id },
                vaga);
        }
        /// <summary>
        /// Lista uma pagina de vagas conforme os parametros
        /// </summary>
        /// <param name="skip">A partir d </param>
        /// <param name="take">Quantos d </param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Caso a listagem de vagas seja feita com sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IEnumerable<VagaResponse> ListaVagas([FromQuery] int skip = 0, [FromQuery] int take = 25)
        {
            return _mapper.Map<List<VagaResponse>>(_context.Vagas.Skip(skip).Take(take));
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
            var vagaResponse = _mapper.Map<VagaResponse>(vaga);

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
        public IActionResult AtualizaVagaCompleta(int id, [FromBody] VagaRequest vagaRequest)
        {
            Vaga? vaga = _context.Vagas.FirstOrDefault(vaga => vaga.Id == id);

            if(vaga == null)
            {
                return NotFound();
            }
            _mapper.Map(vagaRequest, vaga);
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
        public IActionResult AtualizarVaga(int id, [FromBody] JsonPatchDocument<VagaRequest> patch)
        {
            Vaga? vaga = _context.Vagas.FirstOrDefault(vaga => vaga.Id == id);
            if (vaga == null)
            {
                return NotFound();
            }
            var vagaParaAtualizar = _mapper.Map<VagaRequest>(vaga);
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
                List<Vaga> lista = new List<Vaga>();
                for (int i = 0; i < 50; i++)
                {
                    Vaga vagaAberta = new Vaga
                    {
                        Titulo = "vaga" + i,
                        Descricao = "A vaga é para " + i,
                        Tipo = "remoto"
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
}
