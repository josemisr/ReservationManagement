using Api.Models;
using AutoMapper;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            CreateMap<Employees, EmployeeDto>();
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
