using AutoMapper;
using DataAccess.Models;
using ReservationServiceApi.Models;

namespace ReservationServiceApi
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
            CreateMap<Employees, EmployeeDto>();
            CreateMap<EmployeeDto, Employees>();
            CreateMap<EmployeesShifts, EmployeeShiftSimpleDto>();
            CreateMap<EmployeesShifts, EmployeeShiftDto>();
            CreateMap<EmployeeShiftDto, EmployeesShifts>();
            CreateMap<Reservations, ReservationSimpleDto>();
            CreateMap<ServicesEmployees, ServiceEmployeeSimpleDto>();
        }
    }
}
