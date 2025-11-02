using System.ComponentModel.DataAnnotations;

namespace Usuarios.API.Response
{
    public class UsuarioResponse
    {
        public string Nome { get; set; }

        public string Email { get; set; }
        public string Telefone { get; set; }

        public DateTime HoraConsulta { get; set; } = DateTime.Now;
    }
}