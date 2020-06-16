using Api.IServicesApi;
using Api.Models;
using DataAccess.Models;
using DataAccess.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ServicesApi
{
    public class ServiceService:IServiceService
    {
        private ServiceOperations db = new ServiceOperations();
        public List<Services> GetServices()
        {
            return db.getAllServices();
        }
        public Services GetServiceById(int id)
        {
            return db.GetByPk(id);
        }
        public Services AddService(ServiceDto servicesDto)
        {
            Services servicesDb = new Services();
            servicesDb.Id = servicesDto.Id;
            servicesDb.Image = servicesDto.Image;
            servicesDb.Name = servicesDto.Name;
            servicesDb.Price = servicesDto.Price;
            servicesDb.Description = servicesDto.Description;
            var result = db.CreateService(servicesDb);
            return result;
        }
        public Services UpdateService(ServiceDto servicesDto)
        {
            Services servicesDb = new Services();
            servicesDb.Id = servicesDto.Id;
            servicesDb.Image = servicesDto.Image;
            servicesDb.Name = servicesDto.Name;
            servicesDb.Price = servicesDto.Price;
            servicesDb.Description = servicesDto.Description;
            var result = db.UpdateService(servicesDb);
            return result;
        }
        public Services RemoveService(int id)
        {
            var result = db.DeleteService(id);
            return result;
        }
    }
}
