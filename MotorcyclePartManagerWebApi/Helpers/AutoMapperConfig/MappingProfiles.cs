using AutoMapper;
using MotorcyclePartManagerWebApi.Entities;
using MotorcyclePartManagerWebApi.Models;

namespace MotorcyclePartManagerWebApi.Helpers.AutoMapperConfig
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Motorcycle, Motorcycle>();
            CreateMap<Part, Part>();
            CreateMap<Singup, User>();
        }
    }
}
