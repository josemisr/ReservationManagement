﻿using Api.IServicesApi;
using Api.Models;
using AutoMapper;
using DataAccess.Models;
using DataAccess.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ServicesApi
{
    public class EmployeeShiftService:IEmployeeShiftService
    {
        private EmployeeShiftOperations db = new EmployeeShiftOperations();
        private readonly IMapper _mapper;
        public EmployeeShiftService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<EmployeeShiftDto> GetEmployeesShifts()
        {
            var result = db.GetAllEmployeesShifts();
            return _mapper.Map<List<EmployeesShifts>, List<EmployeeShiftDto>>(result);
        }
        public EmployeeShiftDto GetEmployeeShiftById(int id)
        {
            var result= db.GetByPk(id);
            return _mapper.Map<EmployeesShifts, EmployeeShiftDto>(result);
        }
        public EmployeeShiftDto AddEmployeeShift(EmployeeShiftDto employeeShiftDto)
        {
            EmployeesShifts employeesShiftsDb = new EmployeesShifts();
            employeesShiftsDb.Id = employeeShiftDto.Id;
            employeesShiftsDb.IdEmployee = employeeShiftDto.IdEmployee;
            employeesShiftsDb.InitHour = employeeShiftDto.InitHour;
            employeesShiftsDb.WorkDay = employeeShiftDto.WorkDay;
            var result = db.UpdateEmployeeShift(employeesShiftsDb);
            return _mapper.Map<EmployeesShifts, EmployeeShiftDto>(result);
        }
        public EmployeeShiftDto UpdateEmployeeShift(EmployeeShiftDto employeeShiftDto)
        {
            EmployeesShifts employeesShiftsDb = new EmployeesShifts();
            employeesShiftsDb.Id = employeeShiftDto.Id;
            employeesShiftsDb.IdEmployee = employeeShiftDto.IdEmployee;
            employeesShiftsDb.InitHour = employeeShiftDto.InitHour;
            employeesShiftsDb.WorkDay = employeeShiftDto.WorkDay;
            var result = db.UpdateEmployeeShift(employeesShiftsDb);
            return _mapper.Map<EmployeesShifts, EmployeeShiftDto>(result);
        }
        public EmployeeShiftDto RemoveEmployeeShift(int id)
        {
            var result = db.DeleteEmployeeShift(id);
            return _mapper.Map<EmployeesShifts, EmployeeShiftDto>(result);
        }
    }
}
