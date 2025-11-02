using Usuarios.API.Model;
using Usuarios.API.Response;

namespace Usuarios.API.Aplicacao.Interface
{
    public interface IUsuarioService
    {
        Task<Resposta<UsuarioResponse>> CadastrarUsuarioAsync(Usuario usuario);        
        //Resposta<dynamic> Login(Login login);
        //UsuarioToken GerarToken(Login login);
        //Usuario BuscarPorEmail(string email);
    }
}
