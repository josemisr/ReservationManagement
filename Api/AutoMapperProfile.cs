using Api.Models;
using AutoMapper;
using DataAccess.Models;

namespace Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Reservations, ReservationDto>();
            CreateMap<ReservationDto, Reservations>();
            CreateMap<Services, ServiceDto>();
            CreateMap<ServiceDto, Services>();
            CreateMap<Users, UserDto>();
            CreateMap<Users, UserSimpleDto>();
            CreateMap<Employees, EmployeeDto>();
            CreateMap<EmployeeDto, Employees>();
            CreateMap<EmployeesShifts, EmployeeShiftDto>();
            CreateMap<EmployeeShiftDto, EmployeesShifts>();
            CreateMap<EmployeesShifts, EmployeeShiftSimpleDto>();
            CreateMap<Reservations, ReservationSimpleDto>();
            CreateMap<ServicesEmployees, ServiceEmployeeSimpleDto>();
            CreateMap<ServicesEmployees, ServiceEmployeeDto>();
            CreateMap<ServiceEmployeeDto, ServicesEmployees>();
        }
    }
}
