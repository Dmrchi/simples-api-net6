using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Devlivery.API.Response
{
    public class VagaResponse
    {
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string Tipo { get; set; }

        public DateTime HoraConsulta { get; set; } = DateTime.Now;
    }
}
