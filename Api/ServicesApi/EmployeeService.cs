using Api.IServicesApi;
using Api.Models;
using AutoMapper;
using DataAccess.Models;
using DataAccess.Operations;
using System.Collections.Generic;

namespace Api.ServicesApi
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private EmployeeOperations db = new EmployeeOperations();
        public EmployeeService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<EmployeeDto> GetEmployees()
        {
            List<Employees> employees = db.GetAllEmployees();
            List<EmployeeDto> employeesDto = _mapper.Map<List<Employees>, List<EmployeeDto>>(employees);
            return employeesDto;
        }
        public EmployeeDto GetEmployeeById(int id)
        {
            var result = db.GetByPk(id);
            return _mapper.Map<Employees, EmployeeDto>(result);
        }
        public EmployeeDto AddEmployee(EmployeeDto employeeDto)
        {
            Employees employeeDb = _mapper.Map<EmployeeDto, Employees>(employeeDto);
            var result = db.CreateEmployee(employeeDb);
            return _mapper.Map<Employees, EmployeeDto>(result);
        }
        public EmployeeDto UpdateEmployee(EmployeeDto employeeDto)
        {
            Employees employeeDb = _mapper.Map<EmployeeDto, Employees>(employeeDto);
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
