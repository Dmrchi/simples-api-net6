using Usuarios.API.Request;

namespace Usuarios.API.Aplicacao.Interface
{
    public interface IRabbitMqClient
    {
        void PublicaProfissionalParaDevlivery(UsuarioRequest usuarioRequestHttp);
    }
}
