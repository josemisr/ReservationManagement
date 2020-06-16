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
            CreateMap<Employees, EmployeeDto>();
            CreateMap<EmployeesShifts, EmployeeShiftDto>();
            CreateMap<Users, UserDto>();
            CreateMap<EmployeesShifts, EmployeeShiftSimpleDto>();
            CreateMap<Reservations, ReservationSimpleDto>();
            CreateMap<ServicesEmployees, ServiceEmployeeSimpeDto>();
        }
    
}
}
