using Api.IServicesApi;
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
    public class EmployeeService:IEmployeeService
    {
        private readonly IMapper _mapper;
        private EmployeeOperations db = new EmployeeOperations();
        public EmployeeService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<EmployeeDto> GetEmployees()
        {
            List<EmployeeDto> employeesDto = new List<EmployeeDto>();
            List<Employees> employees = db.GetAllEmployees();
            employeesDto = _mapper.Map<List<Employees>, List<EmployeeDto>>(employees);
            return employeesDto;
        }
        public EmployeeDto GetEmployeeById(int id)
        {
            var result= db.GetByPk(id);
            return _mapper.Map<Employees, EmployeeDto>(result);
        }
        public EmployeeDto AddEmployee(EmployeeDto employeeDto)
        {
            Employees employeeDb = new Employees();
            employeeDb.Id = employeeDto.Id;
            employeeDb.IdCard = employeeDto.IdCard;
            employeeDb.Name = employeeDto.Name;
            employeeDb.Surname = employeeDto.Surname;
            employeeDb.Surname2 = employeeDto.Surname2;
            var result = db.CreateEmployee(employeeDb);
            return _mapper.Map<Employees, EmployeeDto>(result);
        }
        public EmployeeDto UpdateEmployee(EmployeeDto employeeDto)
        {
            Employees employeeDb = new Employees();
            employeeDb.Id = employeeDto.Id;
            employeeDb.IdCard = employeeDto.IdCard;
            employeeDb.Name = employeeDto.Name;
            employeeDb.Surname = employeeDto.Surname;
            employeeDb.Surname2 = employeeDto.Surname2;
            var result = db.UpdateEmployee(employeeDb);
            return _mapper.Map<Employees, EmployeeDto>(result);
        }
        public EmployeeDto RemoveEmployee(int id)
        {
            var result = db.DeleteEmployee(id);
            return _mapper.Map<Employees, EmployeeDto>(result);
        }
    }
}
