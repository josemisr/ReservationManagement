using EmployeeServiceApi.Models;
using System.Collections.Generic;

namespace EmployeeServiceApi.IServicesApi
{
    public interface IServiceEmployeeService
    {
        List<ServiceEmployeeDto> GetServicesEmployees();
        ServiceEmployeeDto GetServiceEmployeeById(int id);
        ServiceEmployeeDto AddServiceEmployee(ServiceEmployeeDto serviceDto);
        ServiceEmployeeDto UpdateServiceEmployee(ServiceEmployeeDto serviceDto);
        ServiceEmployeeDto RemoveServiceEmployee(int id);
    }
}
