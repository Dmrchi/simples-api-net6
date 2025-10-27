using AutoMapper;
using Devlivery.API.Request;
using Devlivery.API.Models;
using Devlivery.API.Response;

namespace Devlivery.API.Profiles
{
    public class VagaProfile : Profile
    {
        public VagaProfile()
        {
            CreateMap<VagaRequest, Vaga>();
            CreateMap<Vaga, VagaRequest>();
            CreateMap<Vaga, VagaResponse>();

        }
    }
}
