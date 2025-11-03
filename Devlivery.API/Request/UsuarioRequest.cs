using System.ComponentModel.DataAnnotations;

namespace Devlivery.API.Request
{
    public class UsuarioRequest
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        [MaxLength(100, ErrorMessage = "O tamanho do nome não pode ser maior que 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email do usuário é obrigatório")]
        [MaxLength(50, ErrorMessage = "O tamanho do email do usuário não pode ser maior que 50 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha do usuário é obrigatório")]
        [MaxLength(40, ErrorMessage = "O tamanho da senha do usuário não pode ser maior que 40 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A telefone do usuário é obrigatório")]
        [MaxLength(20, ErrorMessage = "O tamanho do telefone do usuário não pode ser maior que 20 caracteres")]
        public string Telefone { get; set; }
    }
}
