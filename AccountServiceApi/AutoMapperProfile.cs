using AccountServiceApi.Models;
using AutoMapper;
using DataAccess.Models;

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
