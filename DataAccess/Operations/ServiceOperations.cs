using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Operations
{
    public class ServiceOperations
    {
        private ReservationManagementDbContext db = new ReservationManagementDbContext();
        public List<Services> GetAllServices()
        {
            List<Services> services = db.Services.ToList();
            return services;
        }
        public Services CreateService(Services service)
        {
            db.Add(service);
            db.SaveChanges();
            return service;
        }
        public Services DeleteService(int id)
        {
            Services service = GetByPk(id);
            if (service == null) { return null; }
            var servicesEmployeesList = db.ServicesEmployees.Where(elem => elem.IdService == id);
            var reservationsList = db.Reservations.Where(elem => elem.IdService == id);
            foreach (var servicesEmployee in servicesEmployeesList)
            {
                db.Remove(servicesEmployee);
            }
            foreach (var reservation in reservationsList)
            {
                db.Remove(reservation);
            }
            db.Remove(service);
            db.SaveChanges();
            return service;
        }

        public Services UpdateService(Services service)
        {
            db.Update(service);
            db.SaveChanges();
            return service;
        }

        public Services GetByPk(int id)
        {
            var service = db.Services.Find(id);
            return service;
        }
    }
}
