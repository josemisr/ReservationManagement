using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Operations
{
    public class EmployeeOperations
    {
        private ReservationManagementDbContext db = new ReservationManagementDbContext();
        public List<Employees> GetAllEmployees()
        {
            List<Employees> employees = db.Employees.Include(r => r.ServicesEmployees)
                .Include(r => r.EmployeesShifts).ToList();
            return employees;
        }
        public Employees CreateEmployee(Employees employee)
        {
            db.Add(employee);
            db.SaveChanges();
            return employee;
        }
        public Employees DeleteEmployee(int id)
        {
            var employee = GetByPk(id);
            db.Remove(employee);
            db.SaveChanges();
            return employee;
        }

        public Employees UpdateEmployee(Employees employee)
        {
            db.Update(employee);
            db.SaveChanges();
            return employee;
        }

        public Employees GetByPk(int id)
        {
            var employee = db.Employees.Find(id);
            return employee;
        }
    }
}
