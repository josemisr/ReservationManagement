using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.IServicesApi
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
