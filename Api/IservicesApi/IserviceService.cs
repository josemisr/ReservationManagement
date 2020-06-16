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
        List<Services> GetServices();
        Services GetServiceById(int id);
        Services AddService(ServiceDto serviceDto);
        Services UpdateService(ServiceDto serviceDto);
        Services RemoveService(int id);

    }
}
