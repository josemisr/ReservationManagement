using Api.IServicesApi;
using Api.Models;
using AutoMapper;
using DataAccess.Models;
using DataAccess.Operations;
using System.Collections.Generic;

namespace Api.ServicesApi
{
    public class ServiceEmployeeService:IServiceEmployeeService
    {
        private readonly IMapper _mapper;
        private ServiceEmployeeOperations db = new ServiceEmployeeOperations();
        public ServiceEmployeeService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<ServiceEmployeeDto> GetServicesEmployees()
        {
            List<ServiceEmployeeDto> servicesEmployeesDto = new List<ServiceEmployeeDto>();
            List<ServicesEmployees> servicesEmployees = db.GetAllServicesEmployees();
            servicesEmployeesDto = _mapper.Map<List<ServicesEmployees>, List<ServiceEmployeeDto>>(servicesEmployees);
            return servicesEmployeesDto;
        }
        public ServiceEmployeeDto GetServiceEmployeeById(int id)
        {
            var result = db.GetByPk(id);
            return _mapper.Map<ServicesEmployees, ServiceEmployeeDto>(result);
        }
        public ServiceEmployeeDto AddServiceEmployee(ServiceEmployeeDto serviceEmployeeDto)
        {
            ServicesEmployees serviceEmployeeDb = new ServicesEmployees();
            serviceEmployeeDb = _mapper.Map<ServiceEmployeeDto, ServicesEmployees>(serviceEmployeeDto);
            var result = db.CreateServiceemployee(serviceEmployeeDb);
            return _mapper.Map<ServicesEmployees, ServiceEmployeeDto>(result);
        }
        public ServiceEmployeeDto UpdateServiceEmployee(ServiceEmployeeDto serviceEmployeeDto)
        {
            ServicesEmployees serviceEmployeeDb = new ServicesEmployees();
            serviceEmployeeDb = _mapper.Map<ServiceEmployeeDto, ServicesEmployees>(serviceEmployeeDto);
            var result = db.UpdateServiceemployee(serviceEmployeeDb);
            return _mapper.Map<ServicesEmployees, ServiceEmployeeDto>(result);
        }
        public ServiceEmployeeDto RemoveServiceEmployee(int id)
        {
            var result = db.DeleteServiceemployee(id);
            return _mapper.Map<ServicesEmployees, ServiceEmployeeDto>(result);
        }
    }
}
