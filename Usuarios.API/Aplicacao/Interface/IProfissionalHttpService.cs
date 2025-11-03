using Usuarios.API.Request;

namespace Usuarios.API.Aplicacao.Interface
{
    public interface IProfissionalHttpService
    {
        Task EnviaProfissionalParaDevlivery(UsuarioRequest usuarioRequestHttp);
    }
}
