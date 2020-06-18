using Api.IServicesApi;
using Api.Models;
using AutoMapper;
using DataAccess.Models;
using DataAccess.Operations;
using System.Collections.Generic;

namespace Api.ServicesApi
{
    public class ServiceService : IServiceService
    {
        private ServiceOperations db = new ServiceOperations();
        private readonly IMapper _mapper;
        public ServiceService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<ServiceDto> GetServices()
        {
            var result = db.GetAllServices();
            return _mapper.Map<List<Services>, List<ServiceDto>>(result);
        }
        public ServiceDto GetServiceById(int id)
        {
            var result = db.GetByPk(id);
            return _mapper.Map<Services, ServiceDto>(result);
        }
        public ServiceDto AddService(ServiceDto servicesDto)
        {
            Services servicesDb = _mapper.Map<ServiceDto, Services>(servicesDto);
            var result = db.CreateService(servicesDb);
            return _mapper.Map<Services, ServiceDto>(result);
        }
        public ServiceDto UpdateService(ServiceDto servicesDto)
        {
            Services servicesDb = _mapper.Map<ServiceDto, Services>(servicesDto);
            var result = db.UpdateService(servicesDb);
            return _mapper.Map<Services, ServiceDto>(result);
        }
        public ServiceDto RemoveService(int id)
        {
            var result = db.DeleteService(id);
            return _mapper.Map<Services, ServiceDto>(result);
        }
    }
}
