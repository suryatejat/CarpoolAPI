using AutoMapper;
using Carpool.API.DTO.CustomerDTO;
using Carpool.API.DTO.RideDTO;
using Carpool.API.Models;

namespace Carpool.API.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewCustomerDTO, Customer>().ReverseMap();
            CreateMap<GetRideDTO, Ride>().ReverseMap();
        }
    }
}