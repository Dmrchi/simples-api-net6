using Microsoft.EntityFrameworkCore.Update.Internal;
using System.ComponentModel.DataAnnotations;

namespace Devlivery.API.Models
{
    public class Vaga
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título da vaga é obrigatório")]
        [MaxLength(200, ErrorMessage = "O tamanho do título não pode ser maior que 200 caracteres")]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        [Required(ErrorMessage = "O tipo da vaga é obrigatório")]
        public string Tipo { get; set; }
    }
}
// Appsettings,
//  "ConnectionStrings": {
//"DevliveryConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=delivery;Integrated Security=True;TrustServerCertificate=True;"
//  }
//Ferramentas > Gerenciador de Pacotes Nuget > Console do Gerenciador de Pacotes
//Add-Migration CriandoTabelaVaga
//Update-Database

//Como fazer Patch para Update

//[{
//                "path": "/titulo",
//                "op": "replace",
//                "value": "Arquiteto de Sistemas"
//              },
//              {
//    "path": "/descricao",
//                "op": "replace",
//                "value": "Construir arquiteturas de sistemas distribuidos em Cloud"
//              },
//              {
//    "path": "/tipo",
//                "op": "replace",
//                "value": "Home Office"
//            }]
//