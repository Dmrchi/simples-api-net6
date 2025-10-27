using System.ComponentModel.DataAnnotations;

namespace Devlivery.API.Request
{
    public class VagaRequest
    {
        [Required(ErrorMessage = "O título da vaga é obrigatório")]
        [StringLength(200, ErrorMessage = "O tamanho do título não pode ser maior que 200 caracteres")]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        [Required(ErrorMessage = "O tipo da vaga é obrigatório")]
        [StringLength(200, ErrorMessage = "O tipo da vaga não pode ser maior que 200 caracteres")]
        public string Tipo { get; set; }
    }
}

