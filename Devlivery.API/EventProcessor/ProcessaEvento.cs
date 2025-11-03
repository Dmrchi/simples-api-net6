using AutoMapper;
using Devlivery.API.Data.Interface;
using Devlivery.API.EventProcessor.Interface;
using Devlivery.API.Models;
using Devlivery.API.Request;
using System.Text.Json;

namespace Devlivery.API.EventProcessor
{
    public class ProcessaEvento : IProcessaEvento
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public void Processa(string mensagem)
        {
            Console.WriteLine("Processando mensagem de evento...");
            using var scope = _scopeFactory.CreateScope();
            var vagaRepository = scope.ServiceProvider.GetRequiredService<IVagaRepository>();

            var usuarioDto = JsonSerializer.Deserialize<UsuarioRequest>(mensagem);

            var usuario = _mapper.Map<Usuario>(usuarioDto);

            if (!vagaRepository.ExisteVaga(usuario.Id)){

                VagaRequest vagaRequest = new VagaRequest
                {
                    Titulo = "Vaga para " + usuario.Nome,
                    Descricao = "Vaga criada automaticamente para o usuário " + usuario.Nome,
                    Tipo = "Home Office"
                };
                vagaRepository.CreateVaga(vagaRequest);
                vagaRepository.SaveChanges();
            }
        }
    }
}
