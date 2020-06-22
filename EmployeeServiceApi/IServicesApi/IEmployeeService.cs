using EmployeeServiceApi.Models;
using System.Collections.Generic;

namespace EmployeeServiceApi.IServicesApi
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
