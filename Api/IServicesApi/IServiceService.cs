using Api.Models;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
