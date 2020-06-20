using Api.Models;
using System.Collections.Generic;

namespace Api.IServicesApi
{
    public interface IServiceService
    {
        List<ServiceDto> GetServices();
        ServiceDto GetServiceById(int id);
        ServiceDto AddService(ServiceDto serviceDto);
        ServiceDto UpdateService(ServiceDto serviceDto);
        ServiceDto RemoveService(int id);
    }
}
