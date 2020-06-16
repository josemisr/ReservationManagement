﻿using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Operations
{
    public  class ServiceOperations
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
            var service = GetByPk(id);
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
