using AutoMapper;
using Core.Dtos.Identity;
using Core.Entities.Identity;

namespace Core.Mapping.Auth;
public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}
