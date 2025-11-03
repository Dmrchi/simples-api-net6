using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Usuarios.API.Aplicacao.Interface;
using Usuarios.API.Request;

namespace Usuarios.API.Aplicacao
{
    public class ProfissionalHttpService : IProfissionalHttpService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        public ProfissionalHttpService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }
        public async Task EnviaProfissionalParaDevlivery(UsuarioRequest usuarioRequestHttp)
        {
            var conteudoHttp = new StringContent
            (
                JsonSerializer.Serialize(usuarioRequestHttp),
                Encoding.UTF8,
                "application/json"
            );
            await _client.PostAsync(_configuration["DevliveryService"], conteudoHttp);
            throw new NotImplementedException();
        }
    }
}
