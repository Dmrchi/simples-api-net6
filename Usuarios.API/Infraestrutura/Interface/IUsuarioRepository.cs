using Usuarios.API.Model;
using Usuarios.API.Response;

namespace Usuarios.API.Infraestrutura.Interface
{
    public interface IUsuarioRepository
    {
        Task<bool> CadastrarUsuarioAsync(Usuario usuario);
        //Resposta<Usuario> LoginValidar(Usuario usuario);


    }
}
