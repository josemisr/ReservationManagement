using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Operations
{
    public class EmployeeShiftOperations
    {
        private ReservationManagementDbContext db = new ReservationManagementDbContext();
        public List<EmployeesShifts> GetAllEmployeesShifts()
        {
            List<EmployeesShifts> employeeShift = db.EmployeesShifts.ToList();
            return employeeShift;
        }
        public EmployeesShifts CreateEmployeeShift(EmployeesShifts employeeShift)
        {
            db.Add(employeeShift);
            db.SaveChanges();
            return employeeShift;
        }
        public EmployeesShifts DeleteEmployeeShift(int id)
        {
            var employeeShift = GetByPk(id);
            if (employeeShift == null) { return null; }
            db.Remove(employeeShift);
            db.SaveChanges();
            return employeeShift;
        }

        public EmployeesShifts UpdateEmployeeShift(EmployeesShifts employeeShift)
        {
            db.Update(employeeShift);
            db.SaveChanges();
            return employeeShift;
        }

        public EmployeesShifts GetByPk(int id)
        {
            var employeeShift = db.EmployeesShifts.Find(id);
            return employeeShift;
        }
    }
}
