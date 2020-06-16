using Api.Models;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.IServicesApi
{
    public interface IEmployeeService
    {
        List<EmployeeDto> GetEmployees();
        EmployeeDto GetEmployeeById(int id);
        EmployeeDto AddEmployee(EmployeeDto employeeDto);
        EmployeeDto UpdateEmployee(EmployeeDto employeeDto);
        EmployeeDto RemoveEmployee(int id);
    }
}
