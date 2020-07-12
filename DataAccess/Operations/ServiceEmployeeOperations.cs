using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Operations
{
    public class ServiceEmployeeOperations
    {
        private ReservationManagementDbContext db = new ReservationManagementDbContext();
        public List<ServicesEmployees> GetAllServicesEmployees()
        {
            List<ServicesEmployees> servicesEmployees = db.ServicesEmployees.Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdServiceNavigation).ToList();
            return servicesEmployees;
        }
        public ServicesEmployees CreateServiceemployee(ServicesEmployees serviceEmployee)
        {
            db.Add(serviceEmployee);
            db.SaveChanges();
            return serviceEmployee;
        }
        public ServicesEmployees DeleteServiceemployee(int id)
        {
            ServicesEmployees serviceEmployee = GetByPk(id);
            if (serviceEmployee == null) { return null; }
            db.Remove(serviceEmployee);
            db.SaveChanges();
            return serviceEmployee;
        }

        public ServicesEmployees UpdateServiceemployee(ServicesEmployees serviceEmployee)
        {
            db.Update(serviceEmployee);
            db.SaveChanges();
            return serviceEmployee;
        }
        public ServicesEmployees GetByPk(int id)
        {
            var serviceEmployee = db.ServicesEmployees.Find(id);
            return serviceEmployee;
        }
    }
}
