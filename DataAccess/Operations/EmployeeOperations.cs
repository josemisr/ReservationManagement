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
            var servicesEmployeesList = db.ServicesEmployees.Where(elem => elem.IdEmployee == id);
            var employeesShiftsList = db.EmployeesShifts.Where(elem=> elem.IdEmployee ==id);
            var reservationsList = db.Reservations.Where(elem => elem.IdEmployee == id);
            foreach (var employeeShift in employeesShiftsList)
            {
                db.Remove(employeeShift);
            }
            foreach (var servicesEmployee in servicesEmployeesList)
            {
                db.Remove(servicesEmployee);
            }
            foreach (var reservation in reservationsList)
            {
                db.Remove(reservation);
            }

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
