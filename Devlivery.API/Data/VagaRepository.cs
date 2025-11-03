using AutoMapper;
using Devlivery.API.Data.Interface;
using Devlivery.API.Models;
using Devlivery.API.Request;
using Microsoft.EntityFrameworkCore;
using System;

namespace Devlivery.API.Data
{
    public class VagaRepository : IVagaRepository
    {
        private readonly VagaContext _context;
        private IMapper _mapper;

        public VagaRepository(VagaContext context, IMapper mapper)
        {
            _context = context; 
            _mapper = mapper;

        }

        public void CreateVaga(VagaRequest vagaRequest) //int ProfissionalId
        {
            Vaga vaga = _mapper.Map<Vaga>(vagaRequest);
            _context.Vagas.Add(vaga);
            //_context.SaveChanges();

        }

        public bool ExisteVaga(int vagaId)
        {
            return _context.Vagas.Any(vaga => vaga.Id == vagaId);
        }

        public IEnumerable<Vaga> GetAllVagas()
        {
            return _context.Vagas.ToList();
        }

        public Vaga GetVaga(int vagaId) => _context.Vagas
            .Where(vaga => vaga.Id == vagaId).FirstOrDefault();

        public IEnumerable<Vaga> GetVagaa(int vagaId)
        {
            return _context.Vagas
                .Where(vaga => vaga.Id == vagaId);
        }

        public bool VagaExiste(int vagaId)
        {
            return _context.Vagas.Any(vaga => vaga.Id == vagaId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}