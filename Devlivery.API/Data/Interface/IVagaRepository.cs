using Devlivery.API.Models;
using Devlivery.API.Request;
using Microsoft.EntityFrameworkCore;

namespace Devlivery.API.Data.Interface
{
    public interface IVagaRepository
    {
        void CreateVaga(VagaRequest vagaRequest); //int ProfissionalId


        bool ExisteVaga(int vagaId);

        IEnumerable<Vaga> GetAllVagas();

        Vaga GetVaga(int vagaId);

        IEnumerable<Vaga> GetVagaa(int vagaId);

        bool VagaExiste(int vagaId);

        void SaveChanges();
    }
}
