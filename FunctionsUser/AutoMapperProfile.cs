
using AutoMapper;
using DataAccess.Models;
using FunctionsUser.Models;

namespace AccountServiceApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {      
            CreateMap<Users, UserDto>();
            CreateMap<Users, UserSimpleDto>();
        }
    }
}
